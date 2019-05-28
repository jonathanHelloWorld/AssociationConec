using InterativaSystem.Controllers.Network;
using InterativaSystem.Views.HUD.Run;
using Newtonsoft.Json;

namespace InterativaSystem.Views.HUD.Network.Server.Game
{
    public class NetworkGameStartButton : ButtonView
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
            _newNetworkServer.SendMessageToAll("BootstrapStartGame", JsonConvert.SerializeObject(Game));
        }
    }
}