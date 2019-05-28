using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Question
{
    public class QuestionTitleText : TextView
    {
        protected QuestionsData _questionsData;

        protected override void OnStart()
        {
            base.OnStart();

            _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
            _questionsData.OnNewQuestionReady += SetText;
        }

        protected override void ResetView()
        {
            base.ResetView();

            _tx.text = "";
        }

        protected virtual void SetText()
        {
            var question = _questionsData.GetQuestion();

            _tx.text = question.title;
        }
    }
}