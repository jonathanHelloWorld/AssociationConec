using DG.Tweening;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizShowImageOnRight : ImageView
    {
        private QuizController _quiz;
        public Sprite Right, Wrong;
        public float Delay = 3;
        protected override void OnStart()
        {
            base.OnStart();

            _quiz = _bootstrap.GetController(ControllerTypes.Quiz) as QuizController;

            _quiz.OnAnswerFeedback += Show;

            Hide();
        }

        private void Show(bool value)
        {
            _image.sprite = value ? Right : Wrong;

            _image.DOFade(1, 0.4f).Play();

            Invoke("Hide", Delay);
        }
        private void Hide()
        {
            _image.DOFade(0, 0.4f).Play();
        }
    }
}