using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizShowImageFeedback : ImageView
    {
        private QuizController _quizController;
        public Sprite Won;
        public Sprite Lose;



        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.OnGameEnd += ShowFeedBack;
        }

        private void ShowFeedBack()
        {
            if (_sfxController != null && _quizController.QuestionsRight >= _quizController.CutOffMark)
                _sfxController.PlaySound("Win");

            _image.sprite = _quizController.QuestionsRight >= _quizController.CutOffMark ? Won : Lose;
        }
    }
}