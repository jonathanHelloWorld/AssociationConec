using System;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Register
{
    public class RegisterFieldText : TextView
    {
        private RegisterController _registerController;

        public string Field;

        protected override void OnStart()
        {
            base.OnStart();

            _registerController = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!string.IsNullOrEmpty(Field) && !string.IsNullOrEmpty(_registerController.GetActualRegistry(Field)))
            {
                _tx.text = _registerController.GetActualRegistry(Field);
                return;
            }
        }
    }
}