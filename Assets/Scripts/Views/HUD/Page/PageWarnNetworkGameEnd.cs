using UnityEngine;
using System.Collections;
using InterativaSystem.Views.HUD;
using UnityEngine.Networking;
using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD
{
    public class PageWarnNetworkGameEnd : DoOnPageAuto
    {
        NetworkClientController _client;

        // Use this for initialization
        protected override void OnStart()
        {
            base.OnStart();

            _client = _bootstrap.GetController(ControllerTypes.NetworkClient) as NetworkClientController;
        }

        protected override void DoSomething()
        {
            if (_client != null)
                _client.SendMessageToServer("NetworkGameEnded", "");
        }
    }
}