using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Register
{
    [RequireComponent(typeof(Page.Page))]
    public class RegisterSubmitAuto : DoOnPageAuto
    {
        private RegisterController _registerController;

        protected override void OnStart()
        {
            _registerController = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;
        }

        protected override void DoSomething()
        {
            _registerController.Submit();
        }
    }
}