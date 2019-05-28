using InterativaSystem.Controllers.Network;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Network.Server
{
    public class NetworkServerEnableButton : ButtonView
    {
        private NetworkServerController _serverController;
        protected override void OnStart()
        {
            _serverController = _controller as NetworkServerController;
        }
        protected override void OnClick()
        {
            _serverController.TryStartServer();
            _bt.interactable = false;
        }
    }
}