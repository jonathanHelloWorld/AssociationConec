using System;
using InterativaSystem.Controllers.Network;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/Network Connections Data")]
    public class NetworkConnectionsData : DataModel
    {
        [SerializeField]
        protected List<NetworkClientObject> _clients;

        public event GenericController.SimpleEvent OnClientsReset;
        public event NetworkServerController.ClientEvent OnAddClient, OnUpdateClient, OnRemoveClient;

        #region Initialization
        private void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Network;
        }

        protected override void ResetData()
        {
            base.ResetData();

            _clients = new List<NetworkClientObject>();
        }
        protected override void GetReferences()
        {
            base.GetReferences();

            _idsData.OnNewValue += () => NweUidConnection();

#if HAS_SERVER
            //if(_isServer)
                //OnUpdateClient += NetworkSendConnectionToClients;
#endif
        }

        

        #endregion

        #region Get Setters
        //Get
        public bool TryGetConnection(string id, out NetworkClientObject cl)
        {
            cl = null;
            if (_clients.Exists(x => x.uid == id))
            {
                cl = _clients.Find(x => x.uid == id); 
                
                return true;
            }

            return false;
        }
        public NetworkClientObject GetConnection(string id)
        {
            return _clients.Exists(x => x.uid == id) ? _clients.Find(x => x.uid == id) : null;
        }
        public List<NetworkClientObject> GetConnections(NetworkInstanceType type)
        {
            return _clients.FindAll(x => x.networkType == type);
        }
        public List<NetworkClientObject> GetSessionConnections()
        {
            var ids = _idsData.GetActualSessionIds();

            return _clients.FindAll(x => ids.Exists(y => y.uid == x.uid));
        }
        public List<NetworkClientObject> GetSessionConnections(NetworkInstanceType type)
        {
            var ids = _idsData.GetActualSessionIds();

            return _clients.FindAll(x => ids.Exists(y => y.uid == x.uid) && x.networkType == type);
        }

        public List<NetworkClientObject> GetSessionConnections(NetworkInstanceType type, bool show)
        {
            var ids = _idsData.GetActualSessionIds();

            if(show)
                return _clients.FindAll(x => ids.Exists(y => y.uid == x.uid) && x.networkType == type && x.isGamePrepared);
            else
                return _clients.FindAll(x => ids.Exists(y => y.uid == x.uid) && x.networkType == type);
        }

        //Set
        public void AddConnection(NetworkClientObject cl)
        {
#if HAS_SERVER
            if (_isServer)
            {
                if (_clients.Exists(x => x.uid == cl.uid))
                {
                    var i = _clients.FindIndex(x => x.uid == cl.uid);
                    _clients[i] = cl;

                    if (OnUpdateClient != null) OnUpdateClient(cl);
                }
                else
                {
                    _clients.Add(cl);

                    if (OnAddClient != null) OnAddClient(cl);
                }

                try
                {
                    _serverController.SendMessageByClient(cl, "NetworkonConnectFeedBack", "");
                    base.AddNewValue();

                    UpdateClientsData();

                    DebugLog("Added client " + cl.uid + " with connectionId " + cl.connectionId);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());

                    _clients.RemoveAt(_clients.Count - 1);
                }
            }
            else
            {
                _idsData.NewId(cl.uid, _idsData.actualSession, false);

                if (_clients.Exists(x => x.uid == cl.uid))
                {
                    var i = _clients.FindIndex(x => x.uid == cl.uid);
                    _clients[i] = cl;
                    if (OnUpdateClient != null) OnUpdateClient(cl);
                }
                else
                {
                    _clients.Add(cl);
                    if (OnAddClient != null) OnAddClient(cl);
                }
            }
#endif
        }
        public void SetConnectionValues(NetworkClientObject cl)
        {
            if (_clients.Exists(x => x.uid == cl.uid))
            {
                var i = _clients.FindIndex(x => x.uid == cl.uid);
                _clients[i] = cl;

                if (OnUpdateClient != null) OnUpdateClient(cl);

                return;
            }

            Debug.LogError("Ops, some clients may be duplicated");
            _clients.Add(cl);
        }
        private void NweUidConnection()
        {
#if HAS_SERVER
            if(_isServer) return;
            
            var id = _idsData.GetActual();

            if (_clients.Exists(x => x.uid == id.uid))
            {
                return;
            }

            var cl = _clientController.SetThisClientObjectId(id.uid);
            if (cl == null) return;
            _clients.Add(cl);
            _clientController.SendMessageToServer("NetworkAddClient", JsonConvert.SerializeObject(cl));

            if (OnAddClient != null) OnAddClient(cl);
#endif
        }
        [Obsolete]
        public void RemoveConnections()
        {
#if HAS_SERVER
            if (_isServer)
            {
                var clients = new List<NetworkClientObject>();

                for (int i = 0, n = NetworkServer.connections.Count; i < n; i++)
                {
                    if (NetworkServer.connections[i] == null || NetworkServer.connections[i].connectionId == 0)
                        continue; // Server connectionId
                    try
                    {
                        if (_clients.Exists(x => x.connectionId == NetworkServer.connections[i].connectionId))
                        {
                            clients.Add(_clients.Find(x => x.connectionId == NetworkServer.connections[i].connectionId));
                        }
                        _clients.RemoveAll(x => x.connectionId == NetworkServer.connections[i].connectionId);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("connection " + i + " - " + e);
                    }
                }

                var removedClients = new List<NetworkClientObject>(_clients);

                _clients = new List<NetworkClientObject>(clients);

                UpdateClientsData();

                for (int i = 0, n = removedClients.Count; i < n; i++)
                {
                    if (OnRemoveClient != null) OnRemoveClient(removedClients[i]);
                }
            }
            else
            {
                
            }
#endif
        }
        internal void RemoveConnections(NetworkConnection conn)
        {
#if HAS_SERVER
            if (_isServer)
            {
                var session = GetSessionConnections().OrderBy(x=>x.connectionId).ToList();

                if (session.Exists(x => x.connectionId == conn.connectionId))
                {
                    var disconnectedClient = session.Find(x => x.connectionId == conn.connectionId);

                    if (OnRemoveClient != null) OnRemoveClient(disconnectedClient);
                    /*
                    for (int i = disconnectedClient.connectionId-1; i < session.Count; i++)
                    {
                        if (i == 0 && session[i].connectionId == disconnectedClient.connectionId)
                        {
                            session[i].connectionId = -1;
                            if (OnRemoveClient != null) OnRemoveClient(session[i]);
                        }
                        else if (i != 0)
                        {
                            session[i].connectionId--;
                        }
                        else
                        {
                            Debug.LogError("Something is horrifying wrong");
                        }
                    }
                    /**/
                }
                else
                {
                    Debug.LogError("No one has been disconnected, what a mystery!");
                }
            }
            else
            {

            }
#endif
        }
        
        public void ResetClientStates()
        {
            if (_clients == null) return;

            var sessionClients = GetSessionConnections();

            for (int i = 0; i < sessionClients.Count; i++)
            {
                if (sessionClients[i].networkType != NetworkInstanceType.ActiveClient) continue;

                sessionClients[i].isGamePrepared = false;
                sessionClients[i].isGameEnded = false;
                sessionClients[i].isGameStarted = false;

                //if (OnUpdateClient != null) OnUpdateClient(sessionClients[i]);
            }

            if (OnClientsReset != null) OnClientsReset();
        }
        public void ClearData()
        {
            //_clients = new List<NetworkClientObject>();
        }

        internal bool GetInSession(string uid)
        {
            var sessionIds = _idsData.GetActualSessionIds();

            return sessionIds.Exists(x => x.uid == uid);
        }
        #endregion

        #region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(_clients);
        }
        
        public override void DeserializeDataBase(string json)
        {
            _clients = JsonConvert.DeserializeObject<List<NetworkClientObject>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Networking specific methods
#if HAS_SERVER
        #region Server methods

        public void NetworkSendConnectionToClients(NetworkClientObject client)
        {
            _serverController.SendMessageToAll("NetworkReceiveConnectionUpdate", JsonConvert.SerializeObject(client));
        }

        public void UpdateClientsData()
        {
            if(!_isServer) return;

            _serverController.SendMessageToAll("ClearConnectionData", "");

            for (int i = 0; i < _clients.Count; i++)
            {
                _serverController.SendMessageToAll("UpdateConnectionData", JsonConvert.SerializeObject(_clients[i]));
            }
        }
        #endregion

        #region Client methods
        public void UpdateConnectionData(string json)
        {
            if (_isServer) return;

            AddConnection(JsonConvert.DeserializeObject<NetworkClientObject>(json));
        }
        public void ClearConnectionData(string json)
        {
            if (_isServer) return;

            _clients = new List<NetworkClientObject>();
        }
        public void NetworkReceiveConnectionUpdate(string json)
        {
            if (_isServer) return;

            SetConnectionValues(JsonConvert.DeserializeObject<NetworkClientObject>(json));
        }
        #endregion
#endif
        #endregion

    }

    [System.Serializable]
    public class NetworkClientObject
    {
        public string uid;
        public string lastUid;
        public NetworkInstanceType networkType;
        public bool isGamePrepared;
        public bool isGameStarted;
        public bool isGameEnded;
        public int connectionId;

        [Obsolete]
        [HideInInspector]
        public int SelectedId;

        [Obsolete]
        [HideInInspector]
        public bool isPassive;
        [Obsolete]
        [HideInInspector]
        public string id;
        public NetworkClientObject()
        {
            isGamePrepared = false;
            isGameStarted = false;
            isGameEnded = false;
        }
        public NetworkClientObject(string uid, NetworkInstanceType networkType)
        {
            this.uid = uid;
            this.networkType = networkType;

            isGamePrepared = false;
            isGameStarted = false;
            isGameEnded = false;
        }
    }
}