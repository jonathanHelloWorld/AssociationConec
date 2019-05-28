using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceView : GenericView
    {
        protected AudioSource _audioSource;
        protected SoundDatabase _sounds;

        protected bool _isPlaying;

        protected override void OnStart()
        {
            _sounds = _bootstrap.GetModel(ModelTypes.Sound) as SoundDatabase;
        }
        protected override void OnAwake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        protected override void OnUpdate()
        {
            if (_isPlaying && !_audioSource.isPlaying)
            {
                _isPlaying = false;
                OnClipEnd();
            }
        }

        #region PlaySound
        protected void PlaySound()
        {
            _audioSource.Play();
            _isPlaying = true;
        }
        protected void PlaySound(SoundPropierty sound)
        {
            _audioSource.clip = sound.clip;
            _audioSource.volume = sound.volume;
            _audioSource.pitch = sound.pitch;
            _audioSource.spatialBlend = sound.spatialBlend;
            _audioSource.panStereo = sound.stereoPan;
            _audioSource.loop = sound.loop;
            _audioSource.volume = sound.volume;

            PlaySound();
        }
        protected void PlaySound(string sName)
        {
            if (_sounds.SoundPropierties.Exists(x => x.name == sName))
                PlaySound(_sounds.SoundPropierties.Find(x => x.name == sName));
            else
                Debug.Log("Sound Not Found: " + sName);
        }
        protected void PlaySound(int id)
        {
            if (_sounds.SoundPropierties.Exists(x => x.id == id))
                PlaySound(_sounds.SoundPropierties.Find(x => x.id == id));
            else
                Debug.Log("Sound Not Found: " + id);
        }
        #endregion

        protected void PauseSound()
        {
            _audioSource.Pause();
        }
        protected void StopSound()
        {
            _audioSource.Stop();
            _isPlaying = false;
        }
        protected void ResumeSound()
        {
            _audioSource.UnPause();
        }

        protected virtual void OnClipEnd() { }
    }
}