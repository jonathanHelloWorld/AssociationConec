using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Network.Server
{
    public class NetworkQuizShowRight : ButtonView
    {
        private NetworkServerController _newNetworkServer;
        public bool SendToPassive;
        protected override void OnStart()
        {
            _newNetworkServer = _controller as NetworkServerController;
        }
        protected override void OnClick()
        {
            base.OnClick();

            if (SendToPassive)
            {
                _newNetworkServer.SendMessageToType("NetworkShowRightQuestion", "", NetworkInstanceType.ListeningClient);
            }
            else
            {
                _newNetworkServer.SendMessageToAll("NetworkShowRightQuestion", "");
            }
        }
    }
}