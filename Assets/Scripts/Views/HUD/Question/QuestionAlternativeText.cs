using DG.Tweening;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Question
{
    public class QuestionAlternativeText : TextView
    {
        protected QuestionsData _questionsData;
        public int Id;

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

            _tx.enabled = true;

            if (Id < question.alternatives.Count)
                _tx.text = question.alternatives[Id].title;
            else
                _tx.enabled = false;
        }
    }
}