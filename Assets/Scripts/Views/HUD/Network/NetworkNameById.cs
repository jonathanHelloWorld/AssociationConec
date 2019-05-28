using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkNameById : NetworkTextByIdBase
    {
        public string RegisterName;

        private RegisterController _register;

        protected override void OnStart()
        {
            base.OnStart();

            _register = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _register.OnRegistryUpdate += UpdateData;
        }

        protected override void UpdateData()
        {
            base.UpdateData();

            _tx.text = "";

            if (string.IsNullOrEmpty(myUid)) return;
            if (!_network.GetConnection(myUid).isGamePrepared) return;

            var text = "";

            _register.TryGetRegistryValue(myUid, RegisterName, out text);

            _tx.text = text;
        }

        protected override void UpdateData(NetworkClientObject client)
        {
            base.UpdateData(client);

            _tx.text = "";

            if (string.IsNullOrEmpty(myUid)) return;
            if (!_network.GetConnection(myUid).isGamePrepared) return;

            var text = "";

            _register.TryGetRegistryValue(myUid, RegisterName, out text);

            _tx.text = text;
        }
    }
}