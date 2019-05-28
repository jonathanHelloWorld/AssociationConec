using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.WebCam
{
    public class WebCamStartButton : ButtonView
    {
        private WebCamController _webCamController;

        protected override void OnStart()
        {
            base.OnStart();

            _webCamController = _controller as WebCamController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _webCamController.StartWebCam();
        }
    }
}