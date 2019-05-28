using UnityEngine;
using UnityEngine.Networking;

namespace InterativaSystem.Controllers.Network
{
    public class NetworkManagerOverload : NetworkManager
    {
        public delegate void NetworkConnectionEvent(NetworkConnection conn);

        public event NetworkConnectionEvent OnDisconnectFromServer , OnConnectFromServer;
        public event GenericController.SimpleEvent OnServerStarted , OnServerStoped;


        #region Client
        public override void OnClientConnect(NetworkConnection conn)
        {
            //base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            base.OnClientError(conn, errorCode);
        }
        #endregion

        #region Server
        public override void OnStartServer()
        {
            base.OnStartServer();
            
            if (OnServerStarted != null) OnServerStarted();
        }

        public override void OnStopServer()
        {
            base.OnStopServer();

            if (OnServerStoped != null) OnServerStoped();
        }

        public override void OnServerError(NetworkConnection conn, int errorCode)
        {
            base.OnServerError(conn, errorCode);
        }
        
        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
            if (OnConnectFromServer != null) OnConnectFromServer(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            if (OnDisconnectFromServer != null) OnDisconnectFromServer(conn);
        }
        #endregion
    }
}