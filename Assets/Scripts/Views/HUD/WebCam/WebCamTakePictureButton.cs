using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.WebCam
{
    public class WebCamTakePictureButton : ButtonView
    {
        private WebCamController _webCamController;

        public bool takeImediate;
        protected override void OnStart()
        {
            base.OnStart();

            _webCamController = _controller as WebCamController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _webCamController.TakePicture(!takeImediate);
        }
    }
}