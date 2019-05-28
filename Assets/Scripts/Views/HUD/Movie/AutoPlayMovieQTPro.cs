using UnityEngine;

namespace InterativaSystem.Views.HUD.Movie
{
    [RequireComponent(typeof(AVProWindowsMediaMovie))]
    public class AutoPlayMovieQTPro : RawImageView
    {
        [HideInInspector]
        public AVProWindowsMediaMovie Movie;

        public Renderer Renderer;
        protected override void OnStart()
        {
            base.OnStart();

            if (Renderer!=null)
                Renderer.material.mainTexture = Movie.OutputTexture;

            Movie = GetComponent<AVProWindowsMediaMovie>();
            _image.texture = Movie.OutputTexture;

            UnityEngine.Debug.Log("_image.texture : " + _image.texture);
            Movie.Play();
        }
    }
}