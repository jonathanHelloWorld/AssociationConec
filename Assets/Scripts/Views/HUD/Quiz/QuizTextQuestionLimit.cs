using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizTextQuestionLimit : TextView
    {
        private QuizController _quiz;

        public bool GetMax;
        public bool ZeroBased;
        protected override void OnStart()
        {
            base.OnStart();

            _quiz = _controller as QuizController;
            _quiz.OnQuestionDone += UpdateValue;

            UpdateValue();
        }

        protected void UpdateValue()
        {
            //_tx.text = (GetMax ? _quiz.QuestionLimit + (ZeroBased ? -1 : 0) : _quiz._questionCount + (ZeroBased ? 0 : 1)).ToString("00");
            _tx.text = _quiz.QuestionLimit.ToString("00");
        }
    }
}