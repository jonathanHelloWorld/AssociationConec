using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using InterativaSystem.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace InterativaSystem.Controllers.Network
{
    [AddComponentMenu("ModularSystem/Controllers/Network Client Controller")]
    public class NetworkClientController : NetworkController
    {
        public event SimpleEvent OnDisconect, OnConnect;
        [Obsolete]
        [HideInInspector]
        public bool IsPassive;
        protected NetworkClient thisClient;
        protected NetworkClientObject _thisClientObject;

        protected IdsController _idsController;
        
        [HideInInspector]
        public bool IsConnected;
        
        [Obsolete]
        [HideInInspector]
        public List<NetworkClientObject> Clients;

        [Obsolete]
        public override bool IsServer()
        {
            return false;
        }

        #region Initialization
        private void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.NetworkClient;

            _thisType = this.GetType();

            if (PlayerPrefs.GetString("ip") != null)
            {
                _ip = PlayerPrefs.GetString("ip");
            }

            //SaveIdToPlayerPrefs();
        }
        [Obsolete]
        void SaveIdToPlayerPrefs()
        {
            if (networkType == NetworkInstanceType.ListeningClient)
            {
                if (PlayerPrefs.GetString("id") != null)
                {
                    _id = PlayerPrefs.GetString("id");
                }
            }
        }

        protected override void GetReferences()
        {
            base.GetReferences();
            
            _idsController = _bootstrap.GetController(ControllerTypes.Id) as IdsController;

            _networkConnections.OnClientsReset += CallClientsResetEvent;
            _networkConnections.OnAddClient += CallAddClientEvent;
            _networkConnections.OnUpdateClient += CallOnClientStatsChangeEvent;
            _networkConnections.OnRemoveClient += CallRemoveClientEvent;
        }

        public void InitializeClient()
        {
            if (IsConnected) return;

            try
            {
                thisClient = new NetworkClient();
                thisClient.RegisterHandler(MsgType.Connect, OnConnected);
                thisClient.RegisterHandler(MsgType.Disconnect, OnDisnnected);
                thisClient.RegisterHandler(MsgType.Error, OnError);
                thisClient.RegisterHandler(SimpleCustomMessageSendingId, ReceiveMessage);

                DebugLog(_ip);
                thisClient.Connect(_ip, 4444);

                PlayerPrefs.SetString("ip", _ip);
                //PlayerPrefs.SetString("id", _id);
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        #endregion
        
        #region Connection Methods
        void OnConnected(NetworkMessage netMsg) {  }
        void OnDisnnected(NetworkMessage netMsg)
        {
            DebugLog("OnDisnnected");

            IsConnected = thisClient.isConnected;

            if(_thisClientObject!=null)
            _thisClientObject.connectionId = -1;

            if (OnDisconect != null) OnDisconect();
        }
        void OnError(NetworkMessage netMsg)
        {
            Debug.LogError(netMsg.ToString());
        }

        public void NetworkonConnectFeedBack(string json)
        {
            DebugLog("Connected");

            IsConnected = thisClient.isConnected;

            if (IsConnected)
                if (OnConnect != null) OnConnect();
        }

        public void NetworkClientConnectionSuccess(string json)
        {
            var id = JsonConvert.DeserializeObject<NetworkConnection>(json);

            if (_thisClientObject == null)
            {
                _thisClientObject = new NetworkClientObject(_idsController.GetActualId(), networkType);
            }
            else
            {
                SetThisClientObjectId(_idsController.GetActualId());
            }

            _thisClientObject.connectionId = id.connectionId;

            ForceSendMessageToServer("NetworkAddClient", JsonConvert.SerializeObject(_thisClientObject));
        }
        #endregion

        #region Get Setters
        //Getters
        public string GetThisClientObjectId()
        {
            return _thisClientObject.uid;
        }

        //Setters
        public NetworkClientObject SetThisClientObjectId(string uid)
        {
            if (_thisClientObject == null) return null;

            _thisClientObject.lastUid = _thisClientObject.uid;
            _thisClientObject.uid = uid;
            _thisClientObject.networkType = networkType;

            return _thisClientObject;
        }
        public void SetThisClientObject(string uid)
        {
            if (_thisClientObject != null)
            {
                _thisClientObject.uid = uid;
                _thisClientObject.networkType = networkType;
                return;
            }

            _thisClientObject = new NetworkClientObject(uid, networkType);
        }
        #endregion

        public void NetworkGamePrepare(string json)
        {
            var cType = JsonConvert.DeserializeObject<ControllerTypes>(json);

            var c = _bootstrap.GetController(cType);

            c.PrepareGame();
        }

        public void NetworkGameEnd(string json)
        {
            var cType = JsonConvert.DeserializeObject<ControllerTypes>(json);

            var c = _bootstrap.GetController(cType);

            _bootstrap.EndGame(c);
        }

        public void NetworkUpdateClientData(string json)
        {
            NetworkClientObject cl = JsonConvert.DeserializeObject<NetworkClientObject>(json);

            _networkConnections.SetConnectionValues(cl);
        }

        public void SendMessageToServer(string method, string json)
        {
            if(!IsConnected) return;

            var msg = new CustomNetworkMessage(_thisClientObject.uid, method, json);

            //DebugLog(msg.method);
            try
            {
                thisClient.Send(SimpleCustomMessageSendingId, msg);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
        public void ForceSendMessageToServer(string method, string json)
        {
            var msg = new CustomNetworkMessage(_thisClientObject.uid, method, json);

            //DebugLog(msg.method);
            try
            {
                thisClient.Send(SimpleCustomMessageSendingId, msg);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }
}