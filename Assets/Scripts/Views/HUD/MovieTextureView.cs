using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(AudioSource))]
    public class MovieTextureView : RawImageView
    {
#if UNITY_EDITOR_WIN
        protected MovieTexture _movie;
        protected AudioSource _audio;
        protected override void OnAwake()
        {
            base.OnAwake();

            _audio = GetComponent<AudioSource>();
            _movie = _image.texture as MovieTexture;

            if (_movie != null && _movie.audioClip != null)
                _audio.clip = _movie.audioClip;
        }

        protected void Play()
        {
            UnityEngine.Debug.Log("Play");
            _movie.Play();
            _audio.Play();
        }
        protected void Stop()
        {
            _movie.Stop();
            _audio.Stop();
        }
        protected void Pause()
        {
            _movie.Pause();
            _audio.Pause();
        }
        protected void Resume()
        {
            _movie.Play();
            _audio.UnPause();
        }
#endif
    }
}