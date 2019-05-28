using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;

namespace InterativaSystem.Views.HUD.Network.Server.Game
{
    public class NetworkGamePauseButton : ButtonView
    {
        public ControllerTypes Game;
        private NetworkServerController _newNetworkServer;

        protected override void OnStart()
        {
            _newNetworkServer = _controller as NetworkServerController;
        }
        protected override void OnClick()
        {
            base.OnClick();
            _newNetworkServer.SendMessageToAll("BootstrapPauseGame", JsonConvert.SerializeObject(Game));
        }
    }
}