using System.Collections;
using System.Linq;
using DG.Tweening;
using InterativaSystem.Models;
using InterativaSystem.Views.Grid;
using InterativaSystem.Views.HUD.Network.Server;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Conquer Controller")]
    public class ConquerController : GenericController
    {
        private TurningVoteData turningVoteData;
        private GroupsInfo groupsInfo;
        private GridData gridInfo;

        public event SimpleEvent ShowRank, HideRank;

        private QuizTurningController quizTurning;

        private QuestionsData questionsData;

        [HideInInspector]
        public PolygonSquareGridView MapGrid;
        [HideInInspector]
        public GridPoint ActualGrid;

        public Transform InitalPoint;

        public int AreaToConquer;

        public Image Logo;
        public Color MapDeafaltColor;
        public Color ConquestColor;
        public Material MapMaterial, GridMaterial;

#if HAS_SERVER
        public bool UseTurningUDP;
        public bool UseGrid = true;
#endif

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Conquer;
        }

        protected override void OnStart()
        {
            base.OnStart();

#if HAS_SERVER
            if (!UseTurningUDP)
            {
                groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
                turningVoteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
                groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
                quizTurning = _bootstrap.GetController(ControllerTypes.Quiz) as QuizTurningController;

                if (UseGrid)
                {
                    gridInfo = _bootstrap.GetModel(ModelTypes.Grid) as GridData;
                    gridInfo.GridGenerated += OnGenerated;
                }
            }
            else
            {
                turningVoteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
                groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
                quizTurning = _bootstrap.GetController(ControllerTypes.Quiz) as QuizTurningController;

                if (UseGrid)
                {
                    gridInfo = _bootstrap.GetModel(ModelTypes.Grid) as GridData;
                    gridInfo.GridGenerated += OnGenerated;
                }
            }
#else
            turningVoteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
            gridInfo = _bootstrap.GetModel(ModelTypes.Grid) as GridData;
            quizTurning = _bootstrap.GetController(ControllerTypes.Quiz) as QuizTurningController;

            gridInfo.GridGenerated += OnGenerated;
#endif

            questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
        }

        void OnGenerated()
        {
            ActualGrid = gridInfo.grid.GetGridPoint(InitalPoint.position, 1);
            
            StartCoroutine(temp());
        }

        IEnumerator temp()
        {
            while (MapGrid == null)
            {
                yield return null;
            }

            //MapGrid.PaintAera(gridInfo.grid.ExpandArea(192, ActualGrid), 1);
        }

        public int winner;
        #region Game
        public override void PrepareGame()
        {
            base.PrepareGame();
            winner = GetWinner();

            if(winner == -1)
                Debug.LogError("Não foi possivel detectar o vencedor");
        }

        public override void StartGame()
        {
            base.StartGame();
            
            Conquer();
        }
        
        public override void EndGame()
        {
            base.EndGame();
        }
        #endregion

        protected int GetWinner()
        {
#if HAS_SERVER
            if (!_isServer) return -1;
#endif
            var winner = -1;

            for (int i = 0; i < groupsInfo.Groups.Count; i++)
            {
                groupsInfo.Groups[i].Points = 0;
                groupsInfo.Groups[i].TimeSum = 0;
            }

            //Get only one data by GroupIndex
            var distinctValues =
                from cust in turningVoteData.TurningGroup
                group cust by cust.GroupIndex
                into gcust
                select gcust.First();

            for (int i = 0; i < distinctValues.Count(); i++)
            {
                var votes = turningVoteData.TurningGroup.FindAll(x => x.GroupIndex == turningVoteData.TurningGroup[i].GroupIndex);
                int sum = 0;
                float timeSum = 0;

                for (int j = 0; j < votes.Count; j++)
                {
                    if(!turningVoteData.PadsInfo.Exists(x => x.PadId == votes[j].PadId))
                        continue;

                    Debug.Log(quizTurning);

                    if (quizTurning.CheckIfVoteIsRight(turningVoteData.PadsInfo.Find(x => x.PadId == votes[j].PadId).Vote))
                    {
                        sum += 1;
                        timeSum += (float)turningVoteData.PadsInfo.Find(x => x.PadId == votes[j].PadId).Time;
                    }
                }

                groupsInfo.Groups.Find(x => x.Id == turningVoteData.TurningGroup[i].GroupIndex).Points = sum;
                groupsInfo.Groups.Find(x => x.Id == turningVoteData.TurningGroup[i].GroupIndex).TimeSum = timeSum;
            }
            var winners = groupsInfo.Groups.FindAll(x => x.Points > 0).OrderBy(a => a.TimeSum).ToList();
            winners = winners.OrderByDescending(a => a.Points).ToList();
            //result.Sort((x, y) => string.Compare(x.CheckedIn.ToString(), y.CheckedIn.ToString()));

            Debug.Log(winners.Count);
            if (winners.Count > 0)
                winner = winners[0].Id;

            DebugLog("Winner Group is " + winner);
#if HAS_SERVER
            if(!_isServer)
                _clientController.SendMessageToServer("NetworkSetWinner", JsonConvert.SerializeObject(winner));
            else
                _serverController.SendMessageToAll("NetworkSetWinner", JsonConvert.SerializeObject(winner));

            //Get area Size
            UpdateAreaToConquer();
#endif

            return winner;
        }

        protected void Conquer()
        {
            if (groupsInfo.Groups.Exists(x => x.Id == winner))
                groupsInfo.Groups.Find(x => x.Id == winner).AreaPoints += AreaToConquer;

#if HAS_SERVER

            if (UseGrid)
            {
                var area = gridInfo.grid.ExpandArea(AreaToConquer, ActualGrid);
                MapGrid.PaintAera(area, winner);
                ActualGrid = gridInfo.grid.GetClosestGridPoint(ActualGrid);
            }

            if (_isServer)
                turningVoteData.SaveAll();
#else
            var area = gridInfo.grid.ExpandArea(AreaToConquer, ActualGrid);
            MapGrid.PaintAera(area, winner);
            ActualGrid = gridInfo.grid.GetClosestGridPoint(ActualGrid);
#endif

            groupsInfo.Save();

            EndGame();
        }

        public void PaintByPoints()
        {
#if HAS_SERVER
            if (UseGrid)
            {
                StartCoroutine(PaintByPoints_routine());
            }
#endif
        }

        IEnumerator PaintByPoints_routine()
        {
            for (int i = 0, n = groupsInfo.Groups.Count; i < n; i++)
            {
                var areatoConquer = groupsInfo.Groups[i].Points - groupsInfo.Groups[i].AreaPoints;

                Debug.Log(areatoConquer);

                if (areatoConquer > 0)
                {
                    if (gridInfo.grid.GetValidGridPoints(true).Count < areatoConquer)
                    {
                        areatoConquer = gridInfo.grid.GetValidGridPoints(true).Count;
                        groupsInfo.Groups[i].AreaPoints += areatoConquer;

                        var rest = gridInfo.grid.ExpandArea(areatoConquer, ActualGrid);
                        yield return StartCoroutine(MapGrid.AnimatePaintingArea(rest, groupsInfo.Groups[i].Color, 0.2f))
                            ;

                        yield break;
                    }

                    groupsInfo.Groups[i].AreaPoints += areatoConquer;

                    var area = gridInfo.grid.ExpandArea(areatoConquer, ActualGrid);
                    yield return StartCoroutine(MapGrid.AnimatePaintingArea(area, groupsInfo.Groups[i].Color, 0.2f));
                    //MapGrid.PaintAera(area, groupsInfo.Groups[i]);

                    ActualGrid = gridInfo.grid.GetClosestGridPoint(ActualGrid);
                    yield return null;
                }
            }
        }

#if HAS_SERVER
        public void UpdateAreaToConquer()
        {
            AreaToConquer = questionsData.GetQuestion().weight;
        }
        public void CallConquestMap()
        {
            if (_isServer)
                _serverController.SendMessageToAll("NetworkConquestMap", "");
        }
        public void NetworkConquestMap(string json)
        {
            if (UseGrid)
            {
                Logo.DOColor(new Color(1,1,1,1), 0.4f).Play();
                MapMaterial.DOColor(ConquestColor, 0.4f).Play();
                GridMaterial.DOColor(new Color(1, 1, 1, 0), 0.4f).Play();
            }
        }
        public void NetworkSetWinner(string json)
        {
            var winnerSet = JsonConvert.DeserializeObject<int>(json);
            winner = winnerSet;
            DebugLog("SetWinner " + winner);
        }
        public void NetworkSendWinner(int winnerSet)
        {
            winner = winnerSet;
            if (_isServer)
                _serverController.SendMessageToAll("NetworkSetWinner", JsonConvert.SerializeObject(winnerSet));
        }
        public void NetworKSetArea(string json)
        {
            var area = JsonConvert.DeserializeObject<int>(json);
            AreaToConquer = area;
            DebugLog("SetArea " + AreaToConquer);
        }
        public void NetworkSendArea(int area)
        {
            AreaToConquer = area;
            if (_isServer)
                _serverController.SendMessageToAll("NetworKSetArea", JsonConvert.SerializeObject(area));
        }
        public void NetworkPaintByPoints(string area)
        {
            PaintByPoints();
        }

        public void NetworkShowRank(string json)
        {
            DebugLog("ShowRank");
            if (ShowRank != null) ShowRank();
        }
        public void NetworkHideRank(string json)
        {
            DebugLog("HideRank");
            if (HideRank != null) HideRank();
        }
#endif

    }
}