using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;

namespace InterativaSystem.Views.HUD.Network.Server.Game
{
    public class NetworkGamePrepareButton : ButtonView
    {
        public ControllerTypes GameController;
        public NetworkInstanceType ToType = NetworkInstanceType.Null;

        private NetworkServerController _newNetworkServer;

        protected override void OnStart()
        {
            _newNetworkServer = _controller as NetworkServerController;
        }
        protected override void OnClick()
        {
            base.OnClick();

            _newNetworkServer.SendMessageToType("NetworkGamePrepare", JsonConvert.SerializeObject(GameController), ToType);
        }
    }
}