using InterativaSystem.Controllers.Network;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Network.Server
{
    public class NetworkServerEnabledToogle : ScrollToggleView
    {
        private NetworkServerController _serverController;
        protected override void OnStart()
        {
            _serverController = _controller as NetworkServerController;
            _scroll.interactable = true;
        }
        protected override void OnValueChanged(float value)
        {
            switch ((int) value)
            {
                case 1:
                    _bg.color = Color.green;
                    _serverController.TryStartServer();

                    //Hardcoded for the Run game id: 14706
                    _scroll.interactable = false;
                    break;
                case 0:
                    _bg.color = Color.red;
                    _serverController.TryStopServer();
                    break;
            }
        }
    }
}