using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Question
{
    public class QuestionsUsedText : TextView
    {
        protected QuestionsData _questionsData;
        public bool ZeroBased;

        protected override void OnStart()
        {
            base.OnStart();

            _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
            _questionsData.OnNewQuestionReady += SetText;
        }

        protected virtual void SetText()
        {
            _tx.text = (_questionsData.QuestionsUsed.Count + (ZeroBased ? -1 : 0)).ToString("00");
        }
    }
}