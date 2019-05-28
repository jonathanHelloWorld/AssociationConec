using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Score")]
    public class ScoreData : DataModel
    {
        /// <summary>
        /// Delegate para disparar o evento OnScoreDataUpdated
        /// </summary>
        /// <param name="value"></param>
        public delegate void ScoreEvent(ScoreValue value);
        public delegate void ListScoreEvent(List<ScoreValue> listValues);
        private RegisterController _rg;

        public bool SaveToRegistry;

#if HAS_SERVER
        public event GenericController.SimpleEvent OnScoreReceiveEnded;
        private bool isReceiving;
        public bool SedToServerWhenListener;
#endif

        /// <summary>
        /// Evento de data update para os views
        /// </summary>
        public event GenericController.SimpleEvent OnDataUpdated;
        /// <summary>
        /// Evento para os views se atualizarem de novos dados
        /// </summary>
        public event ScoreEvent OnScoreDataUpdated;

        /// <summary>
        /// Lista que guarda os dados do Model
        /// </summary>
        [SerializeField]
        protected List<ScoreValue> Data;

        /// <summary>
        /// Usado para definir a atual posicao da lita Data
        /// </summary>
        protected int actualScore;

        #region Initialization
        private void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Score;

            Data = new List<ScoreValue>();
        }
        protected override void OnStart()
        {
            base.OnStart();

            _rg = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;
        }

        protected override void ResetData()
        {
            base.ResetData();

            Data = new List<ScoreValue>();
            actualScore = -1;
        }
        protected override void GetReferences()
        {
            base.GetReferences();

            actualScore = Data.Count - 1;

            _idsData.OnNewId += AddNewValue;

#if HAS_SERVER
            if (_isServer)
            {
                _serverController.OnClientsReset += CallReset;
                return;
            }

            if (GetDataFromServerOnConnect)
                _clientController.OnConnect += NetworkRequestScoreFromServer;

            if (!SedToServerWhenListener && _clientController.GetInstanceType() == NetworkInstanceType.ListeningClient) return;

            _clientController.OnConnect += () => NetworkSendScoreValueToServer(Data.Last());

            OnNewValue += NetworkSendScoreValueToServer;
            OnScoreDataUpdated += NetworkSendScoreValueToServer;
#endif
        }

        public override void CallReset()
        {
            base.CallReset();
        }
        #endregion

        #region GetSetters
        //Get methods
        public ScoreValue GetActual()
        {
            return Data[actualScore];
        }
        public ScoreValue GetScore(string uid)
        {
            return Data.Exists(x => x.uid == uid) ? Data.Find(x => x.uid == uid) : null;
        }
        public bool TryGetScore(string uid, out ScoreValue value)
        {
            value = null;

            if (Data.Exists(x => x.uid == uid))
            {
                value = Data.Find(x => x.uid == uid);
                return true;
            }

            return false;
        }
        public List<ScoreValue> GetScoreboard()
        {
            //TODO Yuri: Precesso de alto custo, checar no futuro
            var scrbrd = new List<ScoreValue>(Data);

            scrbrd = scrbrd.OrderBy(x => x.time).ToList();
            scrbrd = scrbrd.OrderByDescending(x => x.value).ToList();

            return scrbrd;
        }
        public ScoreValue GetScoreboardValue(int index)
        {
            //TODO Yuri: Precesso de alto custo, checar no futuro
            var scrbrd = new List<ScoreValue>(Data);

            scrbrd = scrbrd.OrderBy(x => x.time).ToList();
            scrbrd = scrbrd.OrderByDescending(x => x.value).ToList();

            return scrbrd[index];
        }
        public List<ScoreValue> GetSessionValues()
        {
            var scrs = new List<ScoreValue>();
            var scsIds = _idsData.GetActualSessionIds();

            scrs = Data.FindAll(x => scsIds.Exists(y => y.uid == x.uid));

            return scrs;
        }
#if HAS_SERVER
        public List<ScoreValue> GetSessionValues(NetworkInstanceType type)
        {
            var _network = GetNetworkController();
            var sscc = _network.GetSessionConnections(type);

            var scrs = new List<ScoreValue>();
            var scsIds = _idsData.GetActualSessionIds();

            scrs = Data.FindAll(x => scsIds.Exists(y => y.uid == x.uid && sscc.Exists(z => z.uid == y.uid)));

            return scrs;
        }

        public List<ScoreValue> GetSessionValues(NetworkInstanceType type, bool show)
        {
            var _network = GetNetworkController();
            var sscc = _network.GetSessionConnections(type, show);

            var scrs = new List<ScoreValue>();
            var scsIds = _idsData.GetActualSessionIds();

            scrs = Data.FindAll(x => scsIds.Exists(y => y.uid == x.uid && sscc.Exists(z => z.uid == y.uid)));

            return scrs;
        }
#endif

        //Set methods
        public void AddNewValue(Id id)
        {
            Data.Add(new ScoreValue(id.uid));
            actualScore = Data.Count-1;

            base.AddNewValue();
        }
        public void AddNewValue(ScoreValue value)
        {
            if (Data.Exists(x => x.uid == value.uid))
            {
                var i = Data.FindIndex(x => x.uid == value.uid);
                Data[i] = value;
            }
            else
            {
                Data.Add(value);

                base.AddNewValue();
            }

#if HAS_SERVER
            if(!isReceiving)
                OnDataUpdateCall(GetActual());
            else
                if (OnDataUpdated != null) OnDataUpdated();
#else
                OnDataUpdateCall(GetActual());
#endif
        }
        public void AddToActualValue(int value)
        {
            Data[actualScore].value += value;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void AddToActualValue(float time)
        {
            Data[actualScore].time += time;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void AddToActualValue(int value, float time)
        {
            Data[actualScore].value += value;
            Data[actualScore].time += time;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void AddToActualValue(int value, float time, int type)
        {
            Data[actualScore].value += value;
            Data[actualScore].time += time;
            Data[actualScore].type = type;

            //Data[actualScore].types[type] += 1;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void SetActualValue(int value)
        {
            Data[actualScore].value = value;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void SetActualValue(float time)
        {
            Data[actualScore].time = time;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void SetActualValue(int value, float time)
        {
            Data[actualScore].value = value;
            Data[actualScore].time = time;

            OnDataUpdateCall(Data[actualScore]);
        }
        public void SetAllValues(List<ScoreValue> Score)
        {
            Data = new List<ScoreValue>(Score);

            actualScore = Data.Count-1;

            if (OnDataUpdated != null) OnDataUpdated();
        }

        //Events
        protected void OnDataUpdateCall(ScoreValue value)
        {
            if (SaveToRegistry)
            {
                _rg.AddRegisterValue("Score", GetActual().value.ToString("000"), false);
                _rg.AddRegisterValue("GameTime", GetActual().time.ToString(), false);
            }

            if (OnDataUpdated != null) OnDataUpdated();
            if (OnScoreDataUpdated != null) OnScoreDataUpdated(value);
        }
        #endregion

        #region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Data, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            Data = JsonConvert.DeserializeObject<List<ScoreValue>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Networking specific methods
#if HAS_SERVER
        #region Server methods
        public void NetworkSendScoreToClient(string id, string json)
        {
            if (!_isServer) return;

            _serverController.SendMessageByClient(id, "NetworkSetScore", JsonConvert.SerializeObject(Data));
        }
        public void NetworkRequestScore(string json)
        {
            //TODO Yuri: o request do ids deve devolver apenas para quem pediu
            if (!_isServer) return;

            _serverController.SendMessageToAll("NetworkUpdateScoreStart", "");

            for (int i = 0; i < Data.Count; i++)
            {
                _serverController.SendMessageToAll("NetworkSetSingleScore", JsonConvert.SerializeObject(Data[i]));
            }

            _serverController.SendMessageToAll("NetworkUpdateScoreEnd", "");

            DebugLog("RequestScore - All Data Send");
        }
        public void NetworkUpdateScore(string json)
        {
            if (!_isServer) return;

            AddNewValue(JsonConvert.DeserializeObject<ScoreValue>(json));

            _serverController.SendMessageToAll("NetworkSetSingleScore", json);
            //_serverController.SendMessageToType("NetworkSetSingleScore", json, NetworkInstanceType.ListeningClient);
        }
        #endregion

        #region Client methods
        public void NetworkSendScoreValueToServer(ScoreValue value)
        {
            if (_isServer || isReceiving) return;

            _clientController.SendMessageToServer("NetworkUpdateScore", JsonConvert.SerializeObject(value));
        }
        private void NetworkSendScoreValueToServer()
        {
            if (_isServer || isReceiving) return;

            _clientController.SendMessageToServer("NetworkUpdateScore", JsonConvert.SerializeObject(Data.Last()));
        }
        public void NetworkSetScore(string json)
        {
            if (_isServer) return;

            SetAllValues(JsonConvert.DeserializeObject<List<ScoreValue>>(json));
        }
        public void NetworkRequestScoreFromServer()
        {
            if (_isServer) return;

            _clientController.SendMessageToServer("NetworkRequestScore", "");
        }

        #region Receive all data from server process
        public void NetworkUpdateScoreStart(string json)
        {
            if (_isServer) return;

            isReceiving = true;

            Data = new List<ScoreValue>();
            actualScore = -1;
        }
        public void NetworkSetSingleScore(string json)
        {
            if(_isServer) return;

            isReceiving = true;
            AddNewValue(JsonConvert.DeserializeObject<ScoreValue>(json));
            isReceiving = false;
        }
        public void NetworkUpdateScoreEnd(string json)
        {
            if(_isServer) return;

            actualScore = Data.Count - 1;

            AddNewValue(_idsData.GetActual());

            isReceiving = false;
            if (OnScoreReceiveEnded != null) OnScoreReceiveEnded();
        }
        #endregion

        #endregion
#endif
        #endregion

    }

    [System.Serializable]
    public class ScoreValue
    {
        public string uid;
        public int value;
        public float time;
        public int type;

        public byte[] types;

        public ScoreValue()
        {
            InitList();
        }
        public ScoreValue(string uid)
        {
            this.uid = uid;

            InitList();
        }
        public ScoreValue(string uid, int value, float time)
        {
            this.uid = uid;
            this.value = value;
            this.time = time;

            InitList();
        }
        public ScoreValue(string uid, int value, float time, int type)
        {
            this.uid = uid;
            this.value = value;
            this.time = time;
            this.type = type;

            InitList();
        }

        private void InitList()
        {
            types = new byte[6];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = 0;
            }
        }
    }
}