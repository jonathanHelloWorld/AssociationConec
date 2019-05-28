using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    [RequireComponent(typeof(Animator))]
    public class AnimationActionEnded :GenericView
    {
        public int Id;
        private Animator _animator;
        protected override void OnAwake()
        {
            base.OnAwake();

            _animator = GetComponent<Animator>();
        }
        protected override void OnStart()
        {
            base.OnStart();
        }

        private void AnimationEnded(int id)
        {
            _animator.SetBool("Start", false);
            _controller.ActionEnded(id);
        }
        private void CallAnimation(int value)
        {

            _animator.SetInteger("ActionId", value);
            _animator.SetBool("Start", true);
        }
    }
}