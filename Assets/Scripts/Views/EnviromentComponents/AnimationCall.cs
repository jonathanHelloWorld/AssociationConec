using System.Collections.Generic;
using InterativaSystem.Controllers.Sound;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    [RequireComponent(typeof(Animator))]
    public class AnimationCall : GenericView
    {
        public List<int> Ids;
        private Animator _animator;

        protected override void OnAwake()
        {
            base.OnAwake();

            _animator = GetComponent<Animator>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CallAnimation;

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;
        }

        private void CallAnimation(int value)
        {
            if (!Ids.Contains(value)) return;

            _animator.SetInteger("ActionId", value);
            _animator.SetBool("Start", true);
        }

        private SFXController _sfx;
        public void PlaySound(string sound)
        {
            _sfx.PlaySound(sound);
        }
    }
}