using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Question
{
    public class QuestionsButtonEnableOnOver : ButtonView
    {
        private QuestionsData _questions;

        protected override void OnStart()
        {
            base.OnStart();
            _questions = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;

            _questions.OnQuestionsOver += Show;
        }

        void Show()
        {
            _bt.interactable = true;
        }
    }
}