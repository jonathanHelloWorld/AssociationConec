using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.WebCam
{
    public class WebCamStopButton : ButtonView
    {
        private WebCamController _webCamController;

        public bool stopImediate;

        protected override void OnStart()
        {
            base.OnStart();

            _webCamController = _controller as WebCamController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if(stopImediate)
                _webCamController.StopWebCam();
            else
                _webCamController.CountStopWebCam();
        }
    }
}