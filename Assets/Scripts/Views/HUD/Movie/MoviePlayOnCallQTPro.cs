using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Movie
{
    [RequireComponent(typeof(AVProWindowsMediaMovie))]
    public class MoviePlayOnCallQTPro : RawImageView
    {
        [HideInInspector]
        public AVProWindowsMediaMovie Movie;

        public int Id;

        public Renderer Renderer;

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CallAnimation;

            if (Renderer != null)
                Renderer.material.mainTexture = Movie.OutputTexture;

            Movie = GetComponent<AVProWindowsMediaMovie>();
            _image.texture = Movie.OutputTexture;
        }

        private void CallAnimation(int value)
        {
            if (Id != value)
            {
                Stop();
                return;
            }

            Play();
        }
        void Play()
        {
            Movie.Start();
            Movie.Play();
        }
        void Stop()
        {
            Movie.Pause();
        }
    }
}