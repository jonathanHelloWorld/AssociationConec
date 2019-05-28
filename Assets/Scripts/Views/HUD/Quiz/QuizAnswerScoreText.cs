using UnityEngine;
using System.Collections;
using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizAnswerScoreText : DynamicText
    {
        QuizController _quizController;

        public int position = 0;
        public float multiplier = 1;

        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.OnRightAnswer += UpdateText;

            _bootstrap.Reset += () => _tx.text = "00";
        }

        void UpdateText(int value)
        {
            UpdateText(value.ToString(format));
        }
    }
}