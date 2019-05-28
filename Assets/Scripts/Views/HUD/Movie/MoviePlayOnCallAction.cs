using UnityEngine;

namespace InterativaSystem.Views.HUD.Movie
{
    public class MoviePlayOnCallAction : MovieTextureView
    {
#if UNITY_EDITOR_WIN
        public int Id;
        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CallAnimation;

            _movie.loop = true;
        }

        private void CallAnimation(int value)
        {
            if (Id != value) return;

            _movie.Play();
        }
#endif
    }
}