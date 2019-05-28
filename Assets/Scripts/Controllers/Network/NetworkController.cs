using System;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.Networking;


namespace InterativaSystem.Controllers.Network
{
    public enum NetworkInstanceType
    {
        Server = 0,
        ListeningClient = 1,
        ActiveClient = 2,
        ControllerClient = 3,
        Null
    }
    public class NetworkController : GenericController
    {
        public delegate void ClientEvent(NetworkClientObject client);
        public delegate void ReceiveEvent(string method, string json);
        public event StringEvent OnCall, SessionIdChanged;
        public event ReceiveEvent OnReceive;
        public event SimpleEvent IpChanged, IdChanged, PortChanged, OnClientsReset;
        public event ClientEvent OnAddClient, OnRemoveClient, OnClientStatsChange;

        protected NetworkConnectionsData _networkConnections;

        [SerializeField]
        protected NetworkInstanceType networkType;
        
        protected const short SimpleCustomMessageSendingId = 150;

        protected Action ActiveMethod;

        public string _ip = "127.0.0.1";
        [Obsolete]
        [HideInInspector]
        public string _id = "1";
        protected int _port = 4444;
        protected string _sessionId;

        #region Initialization
        void Awake()
        {
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            _networkConnections = _bootstrap.GetModel(ModelTypes.Network) as NetworkConnectionsData;
        }

        #endregion
        
        #region GetSetters
        //Getters
        public virtual NetworkInstanceType GetInstanceType()
        {
            return networkType;
        }
        [Obsolete("IsServer is deprecated, please use GetInstanceType instead.")]
        public virtual bool IsServer()
        {
            return false;
        }
        public string GetIp()
        {
            return _ip;
        }
        public int GetPort()
        {
            return _port;
        }
        public string GetSessionId()
        {
            return _sessionId;
        }
        public NetworkClientObject GetConnection(string uid)
        {
            return _networkConnections.GetConnection(uid);
        }
        public List<NetworkClientObject> GetSessionConnections()
        {
            return _networkConnections.GetSessionConnections();
        }

        public List<NetworkClientObject> GetSessionConnections(NetworkInstanceType type)
        {
            return _networkConnections.GetSessionConnections(type);
        }

        public List<NetworkClientObject> GetSessionConnections(NetworkInstanceType type, bool show)
        {
            return _networkConnections.GetSessionConnections(type, show);
        }

        //Setters
        public void SetSessionId(string id)
        {
            _sessionId = id;
            if (SessionIdChanged != null) SessionIdChanged(_sessionId);
        }
        public void SetIp(string ip)
        {
            _ip = ip;
            if (IpChanged != null) IpChanged();
        }
        public void SetPort(int port)
        {
            _port = port;
            if (PortChanged != null) PortChanged();
        }
        [Obsolete]
        public void SetId(string id)
        {
            _id = id;
            if (IdChanged != null) IdChanged();
        }
        #endregion

        protected void CallClientsResetEvent()
        {
            if (OnClientsReset != null) OnClientsReset();
        }
        protected void CallOnClientStatsChangeEvent(NetworkClientObject cl)
        {
            if (OnClientStatsChange != null) OnClientStatsChange(cl);
        }
        protected void CallAddClientEvent(NetworkClientObject cl)
        {
            if (OnAddClient != null) OnAddClient(cl);
        }
        protected void CallRemoveClientEvent(NetworkClientObject cl)
        {
            if (OnRemoveClient != null) OnRemoveClient(cl);
        }

        protected virtual void ReceiveMessage(NetworkMessage netMsg)
        {
            var msg = netMsg.ReadMessage<CustomNetworkMessage>();

            DebugLog("Calling: " + msg.method);

            if (OnCall != null) OnCall(msg.method);
            if (OnReceive != null) OnReceive(msg.method, msg.json);

            var meth = _thisType.GetMethods().ToList();

            if (!meth.Exists(x => x.Name == msg.method)) return;

            try
            {
                _thisType.GetMethod(msg.method).Invoke(this, new[] { msg.json });
                DebugLog("Callied: " + msg.method + ", on " + _thisType);
            }
            catch (Exception e)
            {
                DebugLog(_thisType + "\n\n" + e.ToString());
            }
        }
    }

    public class CustomNetworkMessage : MessageBase
    {
        public string method;
        public string json;
        public string id;

        public CustomNetworkMessage()
        {
        }
        public CustomNetworkMessage(string method, string json)
        {
            this.method = method;
            this.json = json;
        }
        public CustomNetworkMessage(string id, string method, string json)
        {
            this.id = id;
            this.method = method;
            this.json = json;
        }
    }
}