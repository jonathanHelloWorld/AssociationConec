using InterativaSystem.Models;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Controllers.Sound
{
    public class SFXController : SoundController
    {
        public int qttAudioSource = 5;

        List<AudioSource> audioSources;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.SoundSFX;
        }

        protected override void OnStart()
        {
            base.OnStart();
            _soundPropierties = _sounds.SoundPropierties.FindAll(x => x.category == SoundCategory.SFX);

            audioSources = new List<AudioSource>();

            audioSources.Add(_player);

            for (int i = 0; i < qttAudioSource; i++)
            {
                audioSources.Add(gameObject.AddComponent<AudioSource>());
            }
        }

        void FindFreeSource()
        {
            _player = audioSources.Find(x => x.isPlaying == false);
        }

        public override void PlaySound(int id)
        {
            FindFreeSource();

            base.PlaySound(id);
        }

        public override void PlaySound(SoundPropierty sound)
        {
            FindFreeSource();

            base.PlaySound(sound);
        }

        public override void PlaySound(string sName)
        {
            FindFreeSource();

            base.PlaySound(sName);
        }
    }
}