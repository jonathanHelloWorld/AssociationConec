using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using InterativaSystem.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace InterativaSystem.Controllers.Network
{
    public enum NetworkServerStatus
    {
        Off = 0,
        Started = 1,
        Error
    }

    [AddComponentMenu("ModularSystem/Controllers/Network Server Controller")]
    [RequireComponent(typeof(NetworkManagerOverload))]
    public class NetworkServerController : NetworkController
    {
        public delegate void ReceiveIdEvent(string id, string method, string json);

        public event SimpleEvent OnDisconect, OnCreated, StatusChange;
        public event ReceiveIdEvent OnIdReceive;

        public new event StringEvent OnCall;
        public new event ReceiveEvent OnReceive;

        public NetworkServerStatus Status;

        protected bool _isServerRunning;

        private NetworkManagerOverload _manager;

        [Obsolete]
        public List<NetworkClientObject> Clients;
        private NetworkClient hostClient;
        
        #region Initialization
        private void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.NetworkServer;

            networkType = NetworkInstanceType.Server;

            _thisType = this.GetType();
            
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ip = ip.ToString();
                    return;
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            _manager = GetComponent<NetworkManagerOverload>();

            _console = _bootstrap.GetController(ControllerTypes.Console) as ConsoleController;

            _networkConnections.OnClientsReset += CallClientsResetEvent;
            _networkConnections.OnAddClient += CallAddClientEvent;
            _networkConnections.OnUpdateClient += CallOnClientStatsChangeEvent;
            _networkConnections.OnRemoveClient += CallRemoveClientEvent;

            _manager.OnServerStarted += OnServerStart;
            _manager.OnConnectFromServer += OnServerConnect;
            _manager.OnDisconnectFromServer += OnServerDisconnect;
        }

        void InitializeServer()
        {
            try
            {
                _manager.networkPort = _port;

                hostClient = _manager.StartHost();

                DebugLog("");
                DebugLog("listenPort " + NetworkServer.listenPort);
                DebugLog("");
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public void TryStartServer()
        {
            if (_isServerRunning) return;
            InitializeServer();
        }
        public void TryStopServer()
        {
            if (!_isServerRunning) return;
        }

        public void OnServerStart()
        {
            //NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
            //NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
            NetworkServer.RegisterHandler(MsgType.Error, OnServerError);
            NetworkServer.RegisterHandler(MsgType.AddPlayer, OnPlayerAdd);
            NetworkServer.RegisterHandler(MsgType.RemovePlayer, OnPlayerRemove);
            NetworkServer.RegisterHandler(MsgType.ReconnectPlayer, OnPlayerReconnect);
            NetworkServer.RegisterHandler(SimpleCustomMessageSendingId, ReceiveMessage);

            DebugLog("");
            DebugLog("Server Created, ip: " + _ip);
            DebugLog("");

            if (OnCreated != null) OnCreated();

            _isServerRunning = true;
            ChangeStatus(NetworkServerStatus.Started);

            _networkConnections.ClearData();
        }
        protected override void CallReset()
        {
            base.CallReset();
        }
        #endregion
        
        #region Message methods
        public void SendMessageToAll(string method, string json)
        {
            if (!_isServerRunning) return;

            var msg = new CustomNetworkMessage(method, json);
            try
            {
                NetworkServer.SendToAll(SimpleCustomMessageSendingId, msg);
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        public void SendMessageToType(string method, string json, NetworkInstanceType type)
        {
            if (!_isServerRunning) return;

            var msg = new CustomNetworkMessage(method, json);
            try
            {
                var clients = _networkConnections.GetSessionConnections(type);
                for (int i = 0, n = clients.Count; i < n; i++)
                {
                    NetworkServer.SendToClient(clients[i].connectionId, SimpleCustomMessageSendingId, msg);
                }
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }

        [Obsolete("SendMessageToPassive is deprecated, use SendMessageToType instead")]
        public void SendMessageToPassive(string method, string json)
        {
            if (!_isServerRunning) return;

            var msg = new CustomNetworkMessage(method, json);
            try
            {
                var passives = _networkConnections.GetConnections(NetworkInstanceType.ListeningClient);
                for (int i = 0, n = passives.Count; i < n; i++)
                {
                    NetworkServer.SendToClient(passives[i].connectionId, SimpleCustomMessageSendingId, msg);
                }
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        [Obsolete("SendMessageToActive is deprecated, use SendMessageToType instead")]
        public void SendMessageToActive(string method, string json)
        {
            if (!_isServerRunning) return;

            var msg = new CustomNetworkMessage(method, json);
            try
            {
                var actives = Clients.FindAll(x => !x.isPassive);
                for (int i = 0, n = actives.Count; i < n; i++)
                {
                    NetworkServer.SendToClient(actives[i].connectionId, SimpleCustomMessageSendingId, msg);
                }
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }

        public void SendMessageByClient(int connectionId, string method, string json)
        {
            if (!_isServerRunning) return;

            var msg = new CustomNetworkMessage(method, json);
            try
            {
                NetworkServer.SendToClient(connectionId, SimpleCustomMessageSendingId, msg);
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        public void SendMessageByClient(NetworkClientObject client, string method, string json)
        {
            if (!_isServerRunning) return;

            var msg = new CustomNetworkMessage(method, json);
            try
            {
                NetworkServer.SendToClient(client.connectionId, SimpleCustomMessageSendingId, msg);
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        public void SendMessageByClient(string client, string method, string json)
        {
            if (!_isServerRunning) return;

            var cl = _networkConnections.GetConnection(client);
            var msg = new CustomNetworkMessage(method, json);

            try
            {
                NetworkServer.SendToClient(cl.connectionId, SimpleCustomMessageSendingId, msg);
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        #endregion

        #region GetSetters
        [Obsolete("IsServer is deprecated, please use GetInstanceType instead.")]
        public override bool IsServer()
        {
            return true;
        }


        public bool GetInSession(string uid)
        {
            return _networkConnections.GetInSession(uid);
        }

        public void NetworkAddClient(string id, string json)
        {
            try
            {
                var cl = JsonConvert.DeserializeObject<NetworkClientObject>(json);
                
                if (cl.uid == id)
                {
                    _networkConnections.AddConnection(cl);
                }
                else
                {
                    DebugLog("IndentityMissmatch");
                    Debug.LogError("IndentityMissmatch");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                //throw;
            }
        }
        #endregion

        #region Receive methods
        protected override void ReceiveMessage(NetworkMessage netMsg)
        {
            var msg = netMsg.ReadMessage<CustomNetworkMessage>();

            if (OnCall != null) OnCall(msg.method);
            if (OnReceive != null) OnReceive(msg.method, msg.json);
            if (OnIdReceive != null) OnIdReceive(msg.id, msg.method, msg.json);

            //DebugLog(msg.method);
            try
            {
                _thisType.GetMethod(msg.method).Invoke(this, new[] { msg.json });
                DebugLog("Receiving Message: " + msg.method + " Success.");
            }
            catch (Exception)//e)
            {
                //Debug.Log(e.ToString());
            }
            try
            {
                _thisType.GetMethod(msg.method).Invoke(this, new[] { msg.id, msg.json });
                DebugLog("Receiving Message: " + msg.method + " Success.");
            }
            catch (Exception)// e)
            {
                //Debug.Log(e.ToString());
            }
        }

        public void NetworkGamePrepared(string id, string json)
        {
            NetworkClientObject cl;
            if (_networkConnections.TryGetConnection(id, out cl))
            {
                cl.isGamePrepared = true;
                _networkConnections.SetConnectionValues(cl);

                //TODO Yuri: YURI SOCORROOOO!!!1
                SendMessageToAll("NetworkUpdateClientData", JsonConvert.SerializeObject(cl));
            }
        }
        public void NetworkGameStarted(string id, string json)
        {
            NetworkClientObject cl;
            if (_networkConnections.TryGetConnection(id, out cl))
            {
                cl.isGameStarted = true;
                _networkConnections.SetConnectionValues(cl);
            }
        }
        public void NetworkGameEnded(string id, string json)
        {
            NetworkClientObject cl;
            if (_networkConnections.TryGetConnection(id, out cl))
            {
                cl.isGameEnded = true;
                _networkConnections.SetConnectionValues(cl);
            }
        }
        #endregion

        #region Server Events
        protected void OnDisnnected(NetworkMessage netMsg)
        {
            DebugLog("Disconected from server");

            if (OnDisconect != null) OnDisconect();
        }

        protected void OnServerConnect(NetworkConnection conn)
        {
            DebugLog("Client Connected");
            
            if(conn.connectionId != 0)
                FeedbackClient(conn);
        }
        public void FeedbackClient(NetworkConnection conn)
        {
            SendMessageByClient(conn.connectionId, "NetworkClientConnectionSuccess",JsonConvert.SerializeObject(conn));
        }

        [Obsolete]
        protected void OnServerConnect(NetworkMessage netMsg)
        {
            DebugLog("Client Connected");
        }
        [Obsolete]
        protected void OnServerDisconnect(NetworkMessage netMsg)
        {
            DebugLog("Client Disconnected ");

            _networkConnections.RemoveConnections();
        }

        protected void OnServerDisconnect(NetworkConnection conn)
        {
            DebugLog("Client Disconnected ");

            if (conn.connectionId != 0)
                _networkConnections.RemoveConnections(conn);
        }

        protected void OnServerError(NetworkMessage netMsg)
        {
            DebugLog("OnServerError - " + netMsg);

            _isServerRunning = false;
            ChangeStatus(NetworkServerStatus.Error);
        }
        #endregion

        #region Player Events
        protected void OnPlayerAdd(NetworkMessage netMsg)
        {
            DebugLog("OnPlayerAdd");
        }
        protected void OnPlayerRemove(NetworkMessage netMsg)
        {
            DebugLog("OnPlayerRemove");
        }
        protected void OnPlayerReconnect(NetworkMessage netMsg)
        {
            DebugLog("OnPlayerReconnect");
        }
        #endregion

        void ChangeStatus(NetworkServerStatus status)
        {
            Status = status;
            if (StatusChange != null) StatusChange();
        }

        public void ResetClientStates()
        {
            _networkConnections.ResetClientStates();
        }
    }
}