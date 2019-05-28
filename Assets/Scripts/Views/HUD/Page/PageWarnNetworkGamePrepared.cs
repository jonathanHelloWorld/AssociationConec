using UnityEngine;
using System.Collections;
using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD
{
    public class PageWarnNetworkGamePrepared : DoOnPageAuto
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
                _client.SendMessageToServer("NetworkGamePrepared", "");
        }
    }
}
