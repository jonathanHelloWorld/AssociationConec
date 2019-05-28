using UnityEngine;

namespace InterativaSystem.Views.HUD.Run
{
    [RequireComponent(typeof(Animator))]
    public class PlayAnimationOnColision : GenericView
    {
        public string AnimationTriggerName = "Hit";
        private Animator animator;

        protected override void OnStart()
        {
            base.OnStart();

            animator = GetComponent<Animator>();

            _controller.ObstacleCollision += OnCollided;
            UnityEngine.Debug.Log("PlayAnimationOnColision"+_controller);
        }

        private void OnCollided()
        {
            animator.SetTrigger(AnimationTriggerName);
        }
    }
}