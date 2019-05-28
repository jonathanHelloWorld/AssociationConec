using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkConfigPage : CanvasGroupView
    {
        private NetworkClientController _clientController;
        protected override void OnStart()
        {
            _clientController = _controller as NetworkClientController;
            _clientController.OnConnect += Hide;
            _clientController.OnDisconect += Show;

            Show();
        }
    }
}