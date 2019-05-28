using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;

namespace InterativaSystem.Views.HUD
{ 
    [RequireComponent(typeof(Button))]
    public class ButtonNetworkGameEnd : ButtonView
    {
        public ControllerTypes GameController;
        public NetworkInstanceType ToType = NetworkInstanceType.Null;

        NetworkServerController _serverController;

        protected override void OnStart()
        {
            base.OnStart();

            _serverController = _controller as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _serverController.SendMessageToType("BootstrapEndGame", JsonConvert.SerializeObject(GameController), ToType);
        }
    }
}