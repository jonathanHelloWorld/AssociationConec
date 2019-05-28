using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class AddFakeQuestionQuizButton : ButtonView
    {
        QuizController _quiz;
        protected override void OnStart()
        {
            base.OnStart();

            _quiz = _controller as QuizController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _quiz.AddFakeAnswer();
        }
    }
}