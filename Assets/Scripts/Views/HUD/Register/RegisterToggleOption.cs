using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Register
{
    public class RegisterToggleOption : ToggleView
    {
        protected RegisterController _registerController;
        protected RegistrationData _registerData;
        public string DataName;

        protected override void OnStart()
        {
            base.OnStart();

            _toggle.isOn = false;

            _registerController = _controller as RegisterController;
            _registerController.OnSubmit += Submit;

            _registerData = _bootstrap.GetModel(InterativaSystem.ModelTypes.Register) as RegistrationData;

            _registerData.OnNewRegstry += Reset;
        }

        protected override void Toggled(bool arg0)
        {
            base.Toggled(arg0);

            if (_registerController == null) return;

            _registerController.AddRegisterValue(DataName, arg0.ToString(), false);
        }
        

        private void Reset()
        {
            _toggle.isOn = false;
        }

        private void Submit()
        {

        }
    }
}