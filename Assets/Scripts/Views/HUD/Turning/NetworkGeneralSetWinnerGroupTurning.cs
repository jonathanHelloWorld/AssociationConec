using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;

namespace InterativaSystem.Views.HUD.Turning
{
    public class NetworkGeneralSetWinnerGroupTurning : ButtonView
    {
        private NetworkServerController serverController;

        public int Id;

        protected override void OnStart()
        {
            base.OnStart();

            serverController = _bootstrap.GetController(_controllerType) as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            serverController.SendMessageToAll("NetworkSetWinnerGroup", JsonConvert.SerializeObject(Id));
        }
    }
}