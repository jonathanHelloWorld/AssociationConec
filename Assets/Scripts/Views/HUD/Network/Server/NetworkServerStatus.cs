using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Network.Server
{
    public class NetworkServerStatus : TextView
    {
        private NetworkServerController _serverController;
        protected override void OnStart()
        {
            _tx.text = "Off";
            _serverController = _controller as NetworkServerController;
            _serverController.StatusChange += UpdateStatus;
        }

        private void UpdateStatus()
        {
            _tx.text = _serverController.Status.ToString();
        }
    }
}