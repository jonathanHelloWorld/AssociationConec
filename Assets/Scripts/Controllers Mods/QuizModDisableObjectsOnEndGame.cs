using InterativaSystem.Controllers;
using InterativaSystem.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.ControllersMods
{
    public class QuizModDisableObjectsOnEndGame : GenericView
    {
        QuizController _quizController;

        public int activateOnAnswers = 0;
        public GameObject[] gameObjects;

        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.OnGameEnd += CheckQuizAnswers;
            _quizController.Reset += OnReset;
        }

        void OnReset()
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(true);
            }
        }

        void CheckQuizAnswers()
        {
            if(_quizController._questionCount >= activateOnAnswers)
            {
                for(int i = 0; i < gameObjects.Length; i++)
                {
                    gameObjects[i].SetActive(false);
                }
            }
        }
    }
}