using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class LoadTextureFromFile : RawImageView
    {
        private IOController _io;

        public string ImageName;

        protected override void OnStart()
        {
            base.OnStart();

            _io = _controller as IOController;

            GetTexture();
        }

        void GetTexture()
        {
            var tex = new Texture2D(0, 0);
            _io.TryLoad(ImageName, out tex);

            _image.texture = tex;
        }
    }
}