using System;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Id")]
    public class IdsData : DataModel
    {
        [SerializeField]
        protected List<Id> Ids;
        protected int actualId;

#if HAS_SERVER
        public string actualSession;
#endif

        public delegate void IdEvent(Id id);
        public event IdEvent OnNewId;

        [Space]
        public string projectNumber;

#if HAS_SERVER
        public event GenericController.SimpleEvent OnIdsReceiveEnded;
        public event GenericController.SimpleEvent OnSessionUpdate;
        private bool isReceiving;
#endif

        #region Initialization
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Ids;

            if (string.IsNullOrEmpty(projectNumber))
                Debug.LogError("Projeto sem código.");
        }
        protected override void OnStart()
        {
            base.OnStart();
        }

        /// <summary>
        /// Reseta dados e contador
        /// </summary>
        protected override void ResetData()
        {
            Ids = new List<Id>();
            actualId = -1;
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            actualId = Ids.Count - 1;

#if HAS_SERVER
            if (!_isServer)
                _bootstrap.AppStarted += AddNewValue;
            else
            {
                actualSession = Guid.NewGuid().ToString();
                return;
            }

            if (GetDataFromServerOnConnect)
                _clientController.OnConnect += NetworkRequestIdsFromServer;

            _clientController.OnConnect += NetworkSyncSessionFromServer;
            OnNewValue += NetworkSyncSessionFromServer;
#else
            _bootstrap.AppStarted += AddNewValue;
#endif
        }

        public override void CallReset()
        {
            base.CallReset();
        }
        #endregion

        #region GetSetters
        /// <summary>
        /// Retorna o uid da sessao atual
        /// </summary>
        /// <returns>retorna SessionUid</returns>
        public string GetActualSession()
        {
#if HAS_SERVER
            return actualSession;
#else
            return Ids[actualId].SessionUid;
#endif
        }

        public List<Id> GetActualSessionIds()
        {
            var scs = GetActualSession();
            return Ids.FindAll(x=>x.SessionUid == scs);
        }

        /// <summary>
        /// Pega o atual id sendo usado
        /// </summary>
        /// <returns>retorna o id atual</returns>
        public Id GetActual()
        {
            if(Ids[actualId] != null)
                return Ids[actualId];

            Debug.LogError("Id Null Something is very wrong");
            return null;
        }

        public void SetSession(string session)
        {
            Ids[actualId].SessionUid = session;

#if HAS_SERVER
            actualSession = session;

            if (_isServer) { }
            else
            {
                NetworkSendIdToServer();
            }
            
            if (OnSessionUpdate != null) OnSessionUpdate();
#endif
            Save();
        }
        private void SetActualSession(string session)
        {
#if HAS_SERVER
            actualSession = session;
            
            if (OnSessionUpdate != null) OnSessionUpdate();
#endif
        }

        void UpdateActuaId()
        {
            actualId = Ids.Count-1;
        }
        /// <summary>
        /// Cria novo id disparando o evento de novo id
        /// </summary>
        public override void AddNewValue()
        {
            Ids.Add(new Id());
            UpdateActuaId();

            DebugLog("Created Id");
            if (OnNewId != null) OnNewId(GetActual());

            base.AddNewValue();
        }

        /// <summary>
        /// Cria novo id disparando o evento de novo id
        /// </summary>
        public override void AddNewValue(object[] parameters)
        {
            try
            {
                var sessionId = (string)parameters[0];

                Ids.Add(new Id(sessionId, true));
                UpdateActuaId();

                if (OnNewId != null) OnNewId(GetActual());
                base.AddNewValue(parameters);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Cria novo id disparando o evento de novo id
        /// </summary>
        /// <param name="sessionId"> sobrescreve valor no novo sessionId</param>
        public void NewId(string sessionId)
        {
            Ids.Add(new Id(sessionId, true));
            UpdateActuaId();

            if (OnNewId != null) OnNewId(GetActual());
            OnNewValueEvent();
        }
        public void NewId(string uid, string sessionId, bool update)
        {
            if (Ids.Exists(x => x.uid == uid))
            {
                //var i = Ids.FindIndex(x => x.uid == uid);
                //Ids[i].SessionUid = sessionId;
            }
            else
            {
                Ids.Add(new Id(uid, sessionId));

                if (!update) return;
                UpdateActuaId();
            }

            if(!update) return;

            if (OnNewId != null) OnNewId(GetActual());
            OnNewValueEvent();
        }
        public void NewId(Id value)
        {
#if HAS_SERVER
            NewId(value, !isReceiving);
#else
            NewId(value, true);
#endif
            return;
            /*
            if (Ids.Exists(x => x.uid == value.uid))
            {
                var i = Ids.FindIndex(x => x.uid == value.uid);
                Ids[i] = value;
            }
            else
            {
                Ids.Add(value);
#if HAS_SERVER
                if (!isReceiving)
                    UpdateActuaId();
#else
                UpdateActuaId();
#endif
            }


#if HAS_SERVER
            if (isReceiving) return;

            if (!_isServer)
            {
                if (OnNewId != null) OnNewId(GetActual());
                OnNewValueEvent();
            }
#else
            if (OnNewId != null) OnNewId(GetActual());
            OnNewValueEvent();
#endif
            /**/
        }
        public void NewId(Id value , bool update)
        {
            if (Ids.Exists(x => x.uid == value.uid))
            {
                var i = Ids.FindIndex(x => x.uid == value.uid);
                Ids[i] = value;
            }
            else
            {
                Ids.Add(value);
#if HAS_SERVER
                if (update)
                    UpdateActuaId();
#else
                UpdateActuaId();
#endif
            }


#if HAS_SERVER
            if (!update) return;

            if (!_isServer)
            {
                if (OnNewId != null) OnNewId(GetActual());
                OnNewValueEvent();
            }
#else
            if (OnNewId != null) OnNewId(GetActual());
            OnNewValueEvent();
#endif
        }
        /// <summary>
        /// Cria novo id disparando o evento de novo id
        /// </summary>
        /// <param name="sessionId"> sobrescreve valor no novo sessionId </param>
        /// <param name="id"> sobrescreve valor no novo id </param>
        public void NewId(string sessionId, string id)
        {
            Ids.Add(new Id(id, sessionId));
            UpdateActuaId();

            if (OnNewId != null) OnNewId(GetActual());
            OnNewValueEvent();
        }

        public void NewSession()
        {
#if HAS_SERVER
            actualSession = Guid.NewGuid().ToString();

            SendSessionToClients();

            if (OnSessionUpdate != null) OnSessionUpdate();
#endif
        }
#endregion

#region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Ids);
        }
        public override void DeserializeDataBase(string json)
        {
            Ids = JsonConvert.DeserializeObject<List<Id>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            throw new NotImplementedException();
        }
#endregion

#region Networking specific methods
#if HAS_SERVER
#region Server methods
        public void NetworkUpdateIds(string json)
        {
            if (!_isServer) return;

            var id = JsonConvert.DeserializeObject<Id>(json);
            NewId(id);

            Save();
        }
        public void NetworkRequestIds(string json)
        {
            //TODO Yuri: o request do ids deve devolver apenas para quem pediu
            if (!_isServer) return;

            _serverController.SendMessageToAll("NetworkStartRceiveIds", "");

            for (int i = 0, n = Ids.Count; i < n; i++)
            {
                _serverController.SendMessageToAll("NetworkSetIds", JsonConvert.SerializeObject(Ids[i]));
            }

            _serverController.SendMessageToAll("NetworkIdsEnd", "");

            DebugLog("RequestIds - All Data Send");
        }
        public void SendIdToClients(string json)
        {
            if (!_isServer) return;

            _serverController.SendMessageToAll("NetworkSetIds", JsonConvert.SerializeObject(GetActual()));
        }
        public void SendSessionToClients()
        {
            _serverController.SendMessageToAll("NetworkSetActualSession", JsonConvert.SerializeObject(actualSession));
        }
        public void SendSessionToClient(string id, string json)
        {
            if (!_isServer) return;

            _serverController.SendMessageByClient(id, "NetworkSetSession", JsonConvert.SerializeObject(actualSession));

            var sessionIds = GetActualSessionIds();
            for (int i = 0, n = sessionIds.Count; i < n; i++)
            {
                _serverController.SendMessageToAll("NetworkSetIds", JsonConvert.SerializeObject(sessionIds[i]));
            }
            _serverController.SendMessageToAll("NetworkIdsEnd", "");
        }
#endregion

#region Client methods
        private void NetworkSendIdToServer()
        {
            if (_isServer || isReceiving) return;

            _clientController.SendMessageToServer("NetworkUpdateIds", JsonConvert.SerializeObject(GetActual()));
        }
        private void NetworkRequestIdsFromServer()
        {
            if (_isServer) return;

            _clientController.SendMessageToServer("NetworkRequestIds", "");
        }
        private void NetworkSyncSessionFromServer()
        {
            if (_isServer) return;

            _clientController.SendMessageToServer("SendSessionToClient", "");
        }
        public void NetworkSetActualSession(string json)
        {
            if (_isServer) return;

            SetActualSession(JsonConvert.DeserializeObject<string>(json));
        }
        public void NetworkSetSession(string json)
        {
            if (_isServer) return;

            SetSession(JsonConvert.DeserializeObject<string>(json));
        }

#region Receive all data from server process
        public void NetworkStartRceiveIds(string json)
        {
            if (_isServer) return;

            isReceiving = true;
        }
        public void NetworkSetIds(string json)
        {
            if (_isServer) return;

            isReceiving = true;
            NewId(JsonConvert.DeserializeObject<Id>(json));
            isReceiving = false;
        }
        public void NetworkIdsEnd(string json)
        {
            if (_isServer) return;

            Save();
            
            isReceiving = false;
            if (OnIdsReceiveEnded != null) OnIdsReceiveEnded();
        }
#endregion

#endregion
#endif
#endregion

    }

    /// <summary>
    /// Estrutura de dados para administrar os Ids de usuarios
    /// </summary>
    [System.Serializable]
    public class Id
    {
        /// <summary>
        /// id unico usado para referencia de usuarios
        /// </summary>
        public string uid;

        /// <summary>
        /// id unico usado para referencia da partida
        /// </summary>
        public string SessionUid;

        /// <summary>
        /// constructor vazio que cria o id
        /// </summary>
        public Id()
        {
            uid = Guid.NewGuid().ToString();
            SessionUid = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Constructor que cria com um valor setado para o uid e SessionUid
        /// </summary>
        /// <param name="uid"> id unico (gerar usando o Guid.NewGuid())</param>
        /// <param name="isSession"> se verdadeiro vai definir o session id e gerar o uid aleatorio.</param>
        public Id(string uid, bool isSession)
        {
            if (isSession)
            {
                this.uid = uid;
                SessionUid = Guid.NewGuid().ToString();
            }
            else
            {
                this.uid = Guid.NewGuid().ToString();
                this.SessionUid = uid;
            }
        }
        /// <summary>
        /// Constructor que cria com um valor setado para o uid e SessionUid
        /// </summary>
        /// <param name="uid"> id unico (gerar usando o Guid.NewGuid())</param>
        /// <param name="SessionUid"> id de sessao unico (gerar usando o Guid.NewGuid())</param>
        public Id(string uid, string SessionUid)
        {
            this.uid = uid;
            this.SessionUid = SessionUid;
        }
    }

}