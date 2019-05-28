using System;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;

namespace InterativaSystem.Views.HUD.WebCam
{
    public class WebCamView : RawImageView
    {
        private WebCamController _webCamController;

        public bool setNativeSize = true;

        protected override void OnStart()
        {
            base.OnStart();

            _webCamController = _controller as WebCamController;

            _image.texture = _webCamController.WebCam;

            if (setNativeSize)
                _webCamController.OnWebCanStarted += SetNativeSize;
        }

        private void SetNativeSize()
        {
            _image.SetNativeSize();
        }
    }
}