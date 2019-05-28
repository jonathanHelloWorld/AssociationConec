using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Question
{
    public class GetPreviousQuestionButton : ButtonView
    {
        protected QuestionsData _questionsData;
        protected override void OnStart()
        {
            base.OnStart();

            _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _questionsData.PreviousQuestion();
        }
    }
}