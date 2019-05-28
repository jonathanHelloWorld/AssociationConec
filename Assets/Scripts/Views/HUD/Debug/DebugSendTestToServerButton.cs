using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Debug
{
    public class DebugSendTestToServerButton : ButtonView
    {
        private NetworkClientController _clientController;

        protected override void OnStart()
        {
            _clientController = _controller as NetworkClientController;
        }

        protected override void OnClick()
        {
            base.OnClick();
            _clientController.SendMessageToServer("Test","Lorem ipsum dolor");
        }
    }
}