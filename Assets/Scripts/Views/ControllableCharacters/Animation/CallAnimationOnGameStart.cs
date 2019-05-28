using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters.Animation
{
    [RequireComponent(typeof(Animator))]
    public class CallAnimationOnGameStart : GenericView
    {
        public float WaitTime;

        private Animator _animator;
        private bool state;

        protected override void OnStart()
        {
            base.OnStart();

            _controller.OnGameStart += StartGame;
            _controller.OnGameEnd += EndGame;

            _animator = GetComponent<Animator>();
        }

        private void StartGame()
        {
            state = true;
            Invoke("GameEnable", WaitTime);
        }

        private void EndGame()
        {
            state = false;
            Invoke("GameEnable", WaitTime);
        }

        void GameEnable()
        {
            _animator.SetBool("GameStart", state);
            _animator.SetBool("GameEnd", !state);
        }
    }
}