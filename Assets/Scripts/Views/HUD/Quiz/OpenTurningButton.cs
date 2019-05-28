using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class OpenTurningButton : ButtonView
    {
        private QuizTurningController _quizTurningController;
        protected override void OnStart()
        {
            base.OnStart();

            _quizTurningController = _bootstrap.GetController(_controllerType) as QuizTurningController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _quizTurningController.OpenTurningVote();
        }
    }
}