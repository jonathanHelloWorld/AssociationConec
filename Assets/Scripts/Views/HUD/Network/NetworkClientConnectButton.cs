using System.Collections;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkClientConnectButton : ButtonView
    {
        private NetworkClientController _clientController;

        protected override void OnStart()
        {
            _clientController = _controller as NetworkClientController;
        }

        protected override void OnClick()
        {
            base.OnClick();
            _clientController.InitializeClient();

            _bt.interactable = false;
            StartCoroutine("ReenableButton");
        }

        IEnumerator ReenableButton()
        {
            yield return new WaitForSeconds(2);

            _bt.interactable = true;
        }
    }
}