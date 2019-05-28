using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkIPInput : InputView
    {
        private NetworkController _clientController;

        protected override void OnStart()
        {
            _clientController = _controller as NetworkController;

            input.text = _clientController.GetIp();
        }

        protected override void EndEdit(string value)
        {
            _clientController.SetIp(value);
        }
    }
}