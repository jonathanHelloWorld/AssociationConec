using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Services;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Turning Vote")]
    public class TurningVoteData : DataModel
    {
        public List<TurningPad> PadsInfo;
        public List<TurningGroup> TurningGroup;
        public List<TurningGroupResponse> TurningGroupResponse;
        public List<TurningPadResponse> TurningPadResponse;

        public event GenericController.SimpleEvent VoteReceived, VoteUpdated;
        
        private UDPReceive receiver;
        private UDPSender sender;
        
        public bool GetFromUdp = true;

        private GroupsInfo groupsInfo;
        private QuizTurningController quizTurning;
        private QuestionsData questionsData;

#if HAS_SQLITE3
        private SQLite3Service sqlite;
#endif

        public int winnerGroup;
        public int globalWinnerGroup;


        private string groupFileName = "padsGroups";

        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.TurningVote;

            PadsInfo = new List<TurningPad>();
        }
        protected override void OnStart()
        {
            if (!_IOController.TryLoad(this))
            {
                _IOController.Save(this);
            }
#if HAS_SQLITE3
            sqlite = _bootstrap.GetService(ServicesTypes.SQLite3) as SQLite3Service;
#endif

            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
            questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;

            quizTurning = _bootstrap.GetController(ControllerTypes.Quiz) as QuizTurningController;
            quizTurning.ClodeTurning += SaveAll;

            TurningGroupResponse = new List<TurningGroupResponse>();
            TurningPadResponse = new List<TurningPadResponse>();
            LoadAll();
            LoadGroup();

            if (!GetFromUdp) return;

            receiver = _bootstrap.GetService(ServicesTypes.UDPRead) as UDPReceive;
            sender = _bootstrap.GetService(ServicesTypes.UDPSend) as UDPSender;
            receiver.OnPacketReceive += UpdateFromUdp;
        }

        public void ClearVotes()
        {
            PadsInfo = new List<TurningPad>();
        }

        public class GroupWinData
        {
            public float sums;
            public int points;
            public int groupId;
        }
        public int GetWinnerGroup()
        {
#if HAS_SERVER
            if (!_isServer)
            {
                return winnerGroup;
            }
#endif

            var points = new List<GroupWinData>();

            //Get only one data by GroupIndex
            var distinctValues =
                from cust in TurningGroupResponse
                group cust by cust.GroupId
                into gcust
                select gcust.First();

            List<TurningGroupResponse> groupsDistinct = distinctValues.ToList();

            /*
            //Get only one data by GroupIndex
            var distinctValuesQ =
                from cust in TurningGroupResponse
                group cust by cust.QuestionId
                into gcust
                select gcust.First();
            /**/
            //List<TurningGroupResponse> questionsDistinct = distinctValuesQ.ToList();

            for (int j = 0; j < groupsDistinct.Count; j++)
            {
                if (points.Exists(x => x.groupId == groupsDistinct[j].GroupId))
                {
                    points.Find(x => x.groupId == groupsDistinct[j].GroupId).sums =
                        TurningGroupResponse.FindAll(x => x.GroupId == groupsDistinct[j].GroupId)
                            .Sum(x => x.TimeSum);

                    points.Find(x => x.groupId == groupsDistinct[j].GroupId).points =
                        TurningGroupResponse.FindAll(x => x.GroupId == groupsDistinct[j].GroupId)
                            .Sum(x => x.Points);
                }
                else
                {
                    points.Add(new GroupWinData
                    {
                        sums = TurningGroupResponse.FindAll(x => x.GroupId == groupsDistinct[j].GroupId).Sum(x => x.TimeSum),
                        points = TurningGroupResponse.FindAll(x => x.GroupId == groupsDistinct[j].GroupId).Sum(x => x.Points),
                        groupId = groupsDistinct[j].GroupId
                    });
                }
            }
            /*
            for (int i = 0; i < questionsDistinct.Count(); i++)
            {
                //points.Add(TurningGroupResponse.FindAll(x => x.QuestionId == n[i].QuestionId).Sum(x => x.Points));
                DebugLog(questionsDistinct[i].QuestionId + "=>" + points.Last().points.ToString() + " - " + points.Last().sums.ToString());
            }
            /**/

            var winners = points.FindAll(x => x.points == points.Max(y=>y.points));

            if (winners.Count == 0)
            {
                var temp = points.Find(x => x.points == points.Max(y => y.points)); ;
                winners = new List<GroupWinData>();
                winners.Add(temp);
            }
            if (winners.Count > 1)
            {
                winners = winners.FindAll(x => x.sums - points.Min(y=>x.sums) < 0.01f);
                if (winners.Count > 1)
                {
                    var temp = winners.Find(x => x.sums - points.Min(y => x.sums) < 0.01f);
                    winners = new List<GroupWinData>();
                    winners.Add(temp);
                }
            }

            var winnerGr = winners.First().groupId;
#if HAS_SERVER
            _serverController.SendMessageToAll("NetworkSetWinnerGroup", JsonConvert.SerializeObject(winnerGr));
#endif

            DebugLog("Winner Group" + winnerGr);
            return winnerGr;
        }
        
        private void UpdateFromUdp(string json)
        {
            var pad = JsonConvert.DeserializeObject<TurningPad>(json);

            if (PadsInfo.Exists(x => x.PadId == pad.PadId))
            {
                var index = PadsInfo.FindIndex(x => x.PadId == pad.PadId);

                //If pad changes vote
                if(PadsInfo[index].Vote != pad.Vote)
                    PadsInfo[index].Time = pad.Time;

                PadsInfo[index].Vote = pad.Vote;

                //PadsInfo[PadsInfo.FindIndex(x => x.PadId == pad.PadId)] = pad;
            }
            else
            {
                PadsInfo.Add(pad);
            }

            if (VoteReceived != null) VoteReceived();

            AddGroupResponseData(pad);
            AddPadResponseData(pad);
        }

        void LoadGroup()
        {
#if HAS_SQLITE3
            sqlite.CreateTable<TurningGroup>();

            if (sqlite.GetPadsGroups(out TurningGroup))
            {
                _IOController.Save(JsonConvert.SerializeObject(TurningGroup, Formatting.Indented), groupFileName);
            }
            else
            {
                TurningGroup = new List<TurningGroup>();
            }
#endif
        }
        void SaveGroup()
        {
            _IOController.Save(JsonConvert.SerializeObject(TurningGroup, Formatting.Indented), groupFileName);

#if HAS_SQLITE3
            for (int i = 0; i < TurningGroup.Count; i++)
            {
                sqlite.AddPadsGroupsField(TurningGroup[i]);
            }
#endif
        }

#region Response Methods
        public void SaveAll()
        {
            DebugLog("Saving All Turning Responses");

            for (int i = 0; i < PadsInfo.Count; i++)
            {
                AddGroupResponseData(PadsInfo[i]);
                AddPadResponseData(PadsInfo[i]);
            }
            DebugLog("TurningPadResponse " + TurningPadResponse.Count);
            DebugLog("TurningGroupResponse " + TurningGroupResponse.Count);
#if HAS_SQLITE3
            sqlite.DropTurningGroupResponse();
            sqlite.DropTurningPadResponse();
#endif

            StartCoroutine(SaveTurningPadResponse());

            StartCoroutine(SaveTurningGroupResponse());
        }
        public void LoadAll()
        {
            LoadTurningGroupResponse();
            LoadTurningPadResponse();
            //LoadGroup();
        }

        void LoadTurningGroupResponse()
        {
#if HAS_SQLITE3
            sqlite.CreateTable<TurningGroupResponse>();

            if (sqlite.GetTurningGroupResponses(out TurningGroupResponse))
            {
                _IOController.Save(JsonConvert.SerializeObject(TurningGroupResponse, Formatting.Indented), groupFileName);
            }
            else
            {
                TurningGroupResponse = new List<TurningGroupResponse>();
            }
#endif
        }
        IEnumerator SaveTurningGroupResponse()
        {
            _IOController.Save(JsonConvert.SerializeObject(TurningGroupResponse, Formatting.Indented), "TurningGroupResponse.json");

#if HAS_SQLITE3
            for (int i = 0; i < TurningGroupResponse.Count; i++)
            {
                sqlite.AddTurningGroupResponseField(TurningGroupResponse[i]);
                yield return null;
            }
#endif
            yield return null;
        }

        void LoadTurningPadResponse()
        {
#if HAS_SQLITE3
            sqlite.CreateTable<TurningPadResponse>();

            if (sqlite.GetTurningPadResponses(out TurningPadResponse))
            {
                _IOController.Save(JsonConvert.SerializeObject(TurningPadResponse, Formatting.Indented), groupFileName);
            }
            else
            {
                TurningPadResponse = new List<TurningPadResponse>();
            }
#endif
        }
        IEnumerator SaveTurningPadResponse()
        {
            _IOController.Save(JsonConvert.SerializeObject(TurningPadResponse, Formatting.Indented), "TurningPadResponse.json");

#if HAS_SQLITE3
            for (int i = 0; i < TurningPadResponse.Count; i++)
            {
                sqlite.AddTurningPadResponseField(TurningPadResponse[i]);
                yield return null;
            }
#endif
            yield return null;
        }
        
        void AddGroupResponseData(TurningPad pad)
        {
            if(!TurningGroup.Exists(x => x.PadId == pad.PadId)) return;

            var groupIndex = TurningGroup.Find(x => x.PadId == pad.PadId).GroupIndex;

            if (!groupsInfo.Groups.Exists(x => x.Id == groupIndex)) return;

            var group = groupsInfo.Groups.Find(x => x.Id == groupIndex);

            if (TurningGroupResponse.Exists(x => x.GroupId == groupIndex && x.QuestionId == questionsData.GetQuestion().id))
            {
                var index = TurningGroupResponse.FindIndex(x => x.GroupId == groupIndex && x.QuestionId == questionsData.GetQuestion().id);

                TurningGroupResponse[index].Points = group.AreaPoints;
                TurningGroupResponse[index].TimeSum = group.TimeSum;
            }
            else
            {
                TurningGroupResponse.Add(new TurningGroupResponse
                {
                    GroupId = groupIndex,
                    Points = group.AreaPoints,
                    QuestionId = questionsData.GetQuestion().id,
                    TimeSum = group.TimeSum
                });
            }
        }
        void AddPadResponseData(TurningPad pad)
        {
            if (TurningPadResponse.Exists(x => x.PadId == pad.PadId && x.QuestionId == questionsData.GetQuestion().id))
            {
                //If pad changes vote
                if (TurningPadResponse.Find(x => x.PadId == pad.PadId && x.QuestionId == questionsData.GetQuestion().id).Vote == pad.PadId)
                    TurningPadResponse.Find(x => x.PadId == pad.PadId && x.QuestionId == questionsData.GetQuestion().id).Time = (float)pad.Time;

                TurningPadResponse.Find(x => x.PadId == pad.PadId && x.QuestionId == questionsData.GetQuestion().id).Vote = pad.Vote;
            }
            else
            {
                TurningPadResponse.Add(new TurningPadResponse
                {
                    PadId = pad.PadId,
                    QuestionId = questionsData.GetQuestion().id,
                    Time = (float) pad.Time,
                    Vote = pad.Vote
                });
            }
        }
#endregion

        private int actualGroup;
        public void ReccordPadsToGroup(int groupIndex)
        {
            actualGroup = groupIndex;
            receiver.OnPacketReceive += AddToGroup;
            sender.SendPacket("OpenVote");
        }
        public void EndReccordPadsToGroup()
        {
            sender.SendPacket("CloseVote");
            receiver.OnPacketReceive -= AddToGroup;
            SaveGroup();
        }

        void AddToGroup(string json)
        {
            var pad = JsonConvert.DeserializeObject<TurningPad>(json);

            if (TurningGroup.Exists(x => x.PadId == pad.PadId))
            {
                TurningGroup[TurningGroup.FindIndex(x => x.PadId == pad.PadId)] = new TurningGroup
                {
                    GroupIndex = actualGroup,
                    PadId = pad.PadId
                };
            }
            else
            {
                TurningGroup.Add(new TurningGroup
                {
                    GroupIndex = actualGroup,
                    PadId = pad.PadId
                });
            }
        }

        public void OpenTurningVote()
        {
            ClearVotes();
            sender.SendPacket("OpenVote");
        }
        public void CloseTurningVote()
        {
            sender.SendPacket("CloseVote");
        }

        //This methods will serialize an deserialize from a json data
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(PadsInfo, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            PadsInfo = JsonConvert.DeserializeObject<List<TurningPad>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<TurningPad>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (PadsInfo.Exists(x=> x.PadId == data[i].PadId))
                {
                    PadsInfo.Find(x => x.PadId == data[i].PadId).Time = data[i].Time;
                    PadsInfo.Find(x => x.PadId == data[i].PadId).Vote = data[i].Vote;
                }
                else
                {
                    PadsInfo.Add(data[i]);
                }
            }
        }

#if HAS_SERVER
        public void NetworkSendVotes()
        {
            if (_isServer)
            {
                DebugLog("Sending Truning Votes");
                for (int i = 0, n = PadsInfo.Count; i < n; i++) 
                {
                    _serverController.SendMessageToAll("NetworkReceiveTurningVote", JsonConvert.SerializeObject(PadsInfo[i]));
                }

                _serverController.SendMessageToAll("NetworkReceiveTurningVoteEnd", "");
            }
        }
        public void NetworkSetWinnerGroup(string json)
        {
            var win = JsonConvert.DeserializeObject<int>(json);
            winnerGroup = win;
        }
        public void NetworkGlobalSetWinnerGroup(string json)
        {
            var win = JsonConvert.DeserializeObject<int>(json);
            globalWinnerGroup = win;
        }
        public void NetworkReceiveTurningVote(string json)
        {
            var pad = JsonConvert.DeserializeObject<TurningPad>(json);

            if (PadsInfo.Exists(x => x.PadId == pad.PadId))
            {
                PadsInfo.Find(x => x.PadId == pad.PadId).Vote = pad.Vote;
                PadsInfo.Find(x => x.PadId == pad.PadId).Time = pad.Time;
            }
            else
            {
                PadsInfo.Add(pad);
            }
        }
        public void NetworkReceiveTurningVoteEnd(string json)
        {
            if (VoteUpdated != null) VoteUpdated();
        }
#endif
    }

#if HAS_SQLITE3
    public class TurningGroup
    {
        [PrimaryKey]
        public string PadId { get; set; }
        public int GroupIndex { get; set; }
    }
    public class TurningGroupResponse
    {
        public int GroupId { get; set; }
        public int QuestionId { get; set; }
        public int Points { get; set; }
        public float TimeSum { get; set; }
    }
    public class TurningPadResponse
    {
        public string PadId { get; set; }
        public int QuestionId { get; set; }
        public string Vote { get; set; }
        public float Time { get; set; }
    }
#else

    [System.Serializable]
    public class TurningGroup
    {
        public string PadId;
        public int GroupIndex;
    }
    [System.Serializable]
    public class TurningGroupResponse
    {
        public int GroupId;
        public int QuestionId;
        public int Points;
        public float TimeSum;
    }
    [System.Serializable]
    public class TurningPadResponse
    {
        public string PadId;
        public int QuestionId;
        public string Vote;
        public float Time;
    }
#endif

}