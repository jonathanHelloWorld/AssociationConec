using InterativaSystem.Controllers;
using InterativaSystem.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.ControllersMods
{
    public class QuizModEnableCollidersOnEndGame : GenericView
    {
        QuizController _quizController;

        public int activateOnAnswers = 0;
        public Collider[] colliders;

        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.OnGameEnd += CheckQuizAnswers;
            _quizController.Reset += OnReset;
        }

        void OnReset()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

        void CheckQuizAnswers()
        {
            if(_quizController._questionCount >= activateOnAnswers)
            {
                for(int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
                }
            }
        }
    }
}