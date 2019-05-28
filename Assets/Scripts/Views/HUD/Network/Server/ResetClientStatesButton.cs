using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Network.Server
{
    public class ResetClientStatesButton : ButtonView
    {
        private NetworkServerController _server;

        protected override void OnStart()
        {
            base.OnStart();

            _server = _controller as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _server.ResetClientStates();
        }
    }
}