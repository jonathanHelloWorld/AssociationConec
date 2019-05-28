using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizButtonShowFeedback : ButtonView
    {
        protected QuizController _quizController;
        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.OnAnswerFeedback += Disable;
            _quizController.OnClick += Enable;
        }

        private void Disable(bool value)
        {
            _bt.interactable = false;
        }
        private void Enable()
        {
            _bt.interactable = true;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _quizController.SendrightQuestionFeedBack();
        }
    }
}