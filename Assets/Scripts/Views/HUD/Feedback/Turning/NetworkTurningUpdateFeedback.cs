using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Feedback.Turning
{
    public class NetworkTurningUpdateFeedback : ButtonView
    {
        private NetworkServerController controller;
        protected override void OnStart()
        {
            base.OnStart();

            controller = _bootstrap.GetController(_controllerType) as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            controller.SendMessageToAll("NetworkUpdateFeedback", "");
        }
    }
}