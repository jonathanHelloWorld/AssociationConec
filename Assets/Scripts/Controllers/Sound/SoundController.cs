using System.Collections.Generic;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Controllers.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : GenericController
    {
        protected SoundDatabase _sounds;
        protected List<SoundPropierty> _soundPropierties;

        public event SimpleEvent OnClipEnded;

        protected AudioSource _player;

        [HideInInspector] public bool IsPlaying;
        private bool _isPlaying;
        
        protected override void OnStart()
        {
            _sounds = _bootstrap.GetModel(ModelTypes.Sound) as SoundDatabase;
            _player = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (_isPlaying && !_player.isPlaying)
            {
                _isPlaying = false;
                if (OnClipEnded != null) OnClipEnded();
            }

            IsPlaying = _isPlaying;
        }

        #region PlaySound
        public virtual void PlaySound()
        {
            _player.Play();
            _isPlaying = true;
        }
        public virtual void PlaySound(SoundPropierty sound)
        {
            _player.clip = sound.clip;
            _player.volume = sound.volume;
            _player.pitch = sound.pitch;
            _player.spatialBlend = sound.spatialBlend;
            _player.panStereo = sound.stereoPan;
            _player.loop = sound.loop;
            _player.volume = sound.volume;

            PlaySound();
        }
        public virtual void PlaySound(string sName)
        {
            if (_soundPropierties.Exists(x => x.name == sName))
                PlaySound(_soundPropierties.Find(x => x.name == sName));
            else
                DebugLog("Sound Not Found: " + sName);
        }
        public virtual void PlaySound(int id)
        {
            if (_soundPropierties.Exists(x => x.id == id))
                PlaySound(_soundPropierties.Find(x => x.id == id));
            else
                DebugLog("Sound Not Found: " + id);
        }
        #endregion
        public void PauseSound()
        {
            _player.Pause();
        }
        public void StopSound()
        {
            _player.Stop();
            _isPlaying = false;
        }
        public void ResumeSound()
        {
            _player.UnPause();
        }
    }
}