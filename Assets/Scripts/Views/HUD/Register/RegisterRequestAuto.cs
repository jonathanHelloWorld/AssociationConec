using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Views;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Page;
using UnityEngine;

namespace Assets.Scripts.Views.HUD.Register
{
    [RequireComponent(typeof(Page))]
    public class RegisterRequestAuto : DoOnPageAuto
    {
        private NetworkClientController _clientController;

        protected override void OnStart()
        {
            _clientController = _bootstrap.GetController(ControllerTypes.NetworkClient) as NetworkClientController;
        }

        protected override void DoSomething()
        {
            Debug.Log("RequestRegistry");
            _clientController.SendMessageToServer("RequestRegistry", "");
        }
    }
}