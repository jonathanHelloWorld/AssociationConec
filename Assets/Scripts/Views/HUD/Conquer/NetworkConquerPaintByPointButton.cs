using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class NetworkConquerPaintByPointButton : ButtonView
    {
        private NetworkServerController serverController;
        protected override void OnStart()
        {
            base.OnStart();
            serverController = _bootstrap.GetController(_controllerType) as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            serverController.SendMessageToAll("NetworkPaintByPoints", "");
        }
    }
}