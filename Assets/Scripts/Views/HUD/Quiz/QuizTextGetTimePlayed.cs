using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizTextGetTimePlayed : TextView
    {
        private QuizController _quiz;
        protected override void OnStart()
        {
            base.OnStart();

            _quiz = _controller as QuizController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            _tx.text = _quiz.passed.ToString("N");
        }
    }
}