using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Register
{
    public class RegisterSubmitButton : ButtonView
    {
        private RegisterController _registerController;
        protected override void OnStart()
        {
            _registerController = _controller as RegisterController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _registerController.Submit();
        }
    }
}