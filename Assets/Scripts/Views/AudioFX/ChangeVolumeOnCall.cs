using DG.Tweening;
using InterativaSystem.Controllers.Sound;
using UnityEngine;

namespace InterativaSystem.Views.AudioFX
{
    [RequireComponent(typeof(AudioSource))]
    public class ChangeVolumeOnCall : GenericView
    {
        public int Id;
        public float ChangeDuration;
        public float FinalVolume;

        private AudioSource _audioSource;

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _audioSource = GetComponent<AudioSource>();
            _controller.CallGenericAction += ChangeVolume;
        }

        private void ChangeVolume(int value)
        {
            if (Id == value)
            {
                DOTween.To(x => _audioSource.volume = x, _audioSource.volume, FinalVolume, ChangeDuration);
            }
        }
    }
}