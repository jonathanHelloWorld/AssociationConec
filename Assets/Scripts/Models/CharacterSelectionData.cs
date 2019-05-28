using System;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/Character Selection Data")]
    public class CharacterSelectionData : DataModel
    {
        [SerializeField]
        protected List<CharacterInfo> Data;

        public delegate void Characterevent(CharacterInfo ch);
        
        public event GenericController.SimpleEvent OnCharactersReceiveEnded;
        public event Characterevent OnValueUpdated;


#if HAS_SERVER
        private bool isReceiving, doNotClear;
#endif

        #region Initialization
        private void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Characters;
        }

        protected override void ResetData()
        {
            base.ResetData();

            Data = new List<CharacterInfo>();
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            _idsData.OnNewId += AddNewValue;

#if HAS_SERVER
            if (_isServer)
            {
                _serverController.OnClientsReset += CallReset;
                return;
            }

            if (GetDataFromServerOnConnect)
                _clientController.OnConnect += NetworkRequestCharactersFromServer;

            if (_clientController.GetInstanceType() == NetworkInstanceType.ListeningClient) return;

            OnNewValue += () => NetworkSendCharacterValueToServer(Data.Last());
            OnValueUpdated += NetworkSendCharacterValueToServer;
#endif
        }

        #endregion

        #region Get Setters
        //Get
        public bool TryGetSelectedCharacter(string uid, out CharacterInfo ch)
        {
            ch = null;
            if (Data.Exists(x => x.uid == uid))
            {
                ch = Data.Find(x => x.uid == uid);
                return true;
            }
            return false;
        }
        public CharacterInfo GetSelectedCharacter(string uid)
        {
            if (Data.Exists(x => x.uid == uid))
            {
                return Data.Find(x => x.uid == uid);
            }
            return null;
        }
        public CharacterInfo GetSelectedCharacter()
        {
            var uid = _idsData.GetActual().uid;
            if (Data.Exists(x => x.uid == uid))
            {
                return Data.Find(x => x.uid == uid);
            }
            return null;
        }
        public List<CharacterInfo> GetSessionCharacters()
        {
#if HAS_SERVER
            var net = _bootstrap.GetModel(ModelTypes.Network) as NetworkConnectionsData;
            var session = net.GetSessionConnections(NetworkInstanceType.ActiveClient);

            //return Data.FindAll(x => session.Exists(y => y.uid == x.uid && y.networkType == NetworkInstanceType.ActiveClient));
            return Data.FindAll(x => session.Exists(y => y.uid == x.uid));
#endif
            return null;
        }

        //Set
        public void AddNewValue(Id id)
        {
            Data.Add(new CharacterInfo(id.uid));

            base.AddNewValue();
        }
        public void AddNewValue(CharacterInfo ch)
        {
            if (Data.Exists(x => x.uid == ch.uid))
            {
                var i = Data.FindIndex(x => x.uid == ch.uid);
                Data[i] = ch;

                if (OnValueUpdated != null) OnValueUpdated(ch);
                return;
            }

            Data.Add(ch);

#if HAS_SERVER
            if(isReceiving) return;
#endif

            base.AddNewValue();
        }

        public void SetIdValue(int id)
        {
            var uid = _idsData.GetActual().uid;

            if (Data.Exists(x => x.uid == uid))
            {
                Data.Find(x => x.uid == uid).id = id;
                if (OnValueUpdated != null) OnValueUpdated(Data.Find(x => x.uid == uid));
                return;
            }

            Debug.LogError("Specifield id not exists");
        }
        public void SetIdValue(string uid, int id)
        {
            if (Data.Exists(x => x.uid == uid))
            {
                Data.Find(x => x.uid == uid).id = id;
                if (OnValueUpdated != null) OnValueUpdated(Data.Find(x => x.uid == uid));
                return;
            }

            Debug.LogError("Specifield id not exists");
        }
        public void SetValues(int id, int prefabId, PrefabCategory prefabCategory)
        {
            var uid = _idsData.GetActual().uid;
            if (Data.Exists(x => x.uid == uid))
            {
                Data.Find(x => x.uid == uid).id = id;
                Data.Find(x => x.uid == uid).prefabId = prefabId;
                Data.Find(x => x.uid == uid).prefabCategory = prefabCategory;
                if (OnValueUpdated != null) OnValueUpdated(Data.Find(x => x.uid == uid));
                return;
            }

            Debug.LogError("Specifield id not exists");
        }
        public void SetValues(string uid, int id, int prefabId, PrefabCategory prefabCategory)
        {
            if (Data.Exists(x => x.uid == uid))
            {
                Data.Find(x => x.uid == uid).id = id;
                Data.Find(x => x.uid == uid).prefabId = prefabId;
                Data.Find(x => x.uid == uid).prefabCategory = prefabCategory;
                if (OnValueUpdated != null) OnValueUpdated(Data.Find(x => x.uid == uid));
                return;
            }

            Debug.LogError("Specifield id not exists");
        }

        #endregion

        #region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Data);
        }
        public override void DeserializeDataBase(string json)
        {
            Data = JsonConvert.DeserializeObject<List<CharacterInfo>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Networking specific methods
#if HAS_SERVER
        #region Server methods
        public void NetworkRequestCharacters(string json)
        {
            //TODO Yuri: o request do ids deve devolver apenas para quem pediu
            if (!_isServer) return;

            _serverController.SendMessageToAll("NetworkUpdateCharactersStart", "");

            for (int i = 0; i < Data.Count; i++)
            {
                _serverController.SendMessageToAll("NetworkSetSingleCharacters", JsonConvert.SerializeObject(Data[i]));
            }

            _serverController.SendMessageToAll("NetworkUpdateCharactersEnd", "");

            DebugLog("Request Characters - All Data Send");
        }
        public void NetworkUpdateCharacter(string json)
        {
            if (!_isServer) return;

            AddNewValue(JsonConvert.DeserializeObject<CharacterInfo>(json));

            _serverController.SendMessageToType("NetworkSetSingleCharacters", json, NetworkInstanceType.ListeningClient);
        }
        #endregion

        #region Client methods
        public void NetworkSendCharacterValueToServer(CharacterInfo value)
        {
            if (_isServer || isReceiving) return;

            _clientController.SendMessageToServer("NetworkUpdateCharacter", JsonConvert.SerializeObject(value));
        }
        public void NetworkRequestCharactersFromServer()
        {
            if (_isServer) return;

            _clientController.SendMessageToServer("NetworkRequestCharacters", "");
        }
        public void NetworkUpdateCharactersFromServer()
        {
            if (_isServer) return;
            doNotClear = true;
            _clientController.SendMessageToServer("NetworkRequestCharacters", "");
        }


        #region Receive all data from server process
        public void NetworkUpdateCharactersStart(string json)
        {
            if (_isServer) return;

            isReceiving = true;

            if (doNotClear)
            {
                doNotClear = false;
                return;
            }

            ResetData();
        }
        public void NetworkSetSingleCharacters(string json)
        {
            if (_isServer) return;

            isReceiving = true;
            AddNewValue(JsonConvert.DeserializeObject<CharacterInfo>(json));
            isReceiving = false;
        }
        public void NetworkUpdateCharactersEnd(string json)
        {
            if (_isServer) return;

            AddNewValue(_idsData.GetActual());

            isReceiving = false;
            if (OnCharactersReceiveEnded != null) OnCharactersReceiveEnded();
        }
        #endregion
        #endregion
#endif
        #endregion
    }

    [System.Serializable]
    public class CharacterInfo
    {
        public string uid;
        public int id;
        public int prefabId;
        public PrefabCategory prefabCategory;

        public CharacterInfo(){ }
        public CharacterInfo(string uid)
        {
            this.uid = uid;
        }
    }
}