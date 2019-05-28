using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.DynamicQuiz
{
    public class DynamicQuizAnswerText : TextView
    {
        private DynamicQuizPointBased _dynamicQuiz;
        protected override void OnStart()
        {
            base.OnStart();
            _dynamicQuiz = _controller as DynamicQuizPointBased;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            _tx.text = _dynamicQuiz.Answer.ToString("00");
        }
    }
}