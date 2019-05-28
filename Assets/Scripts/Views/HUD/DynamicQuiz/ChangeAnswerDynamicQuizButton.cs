using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.DynamicQuiz
{
    public class ChangeAnswerDynamicQuizButton : ButtonView
    {
        public bool AddValue;
        public int Value;

        private DynamicQuizPointBased _dynamicQuiz;

        protected override void OnStart()
        {
            base.OnStart();
            _dynamicQuiz = _controller as DynamicQuizPointBased;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (AddValue)
            {
                if(_dynamicQuiz.LimitMaxAnswer >= _dynamicQuiz.Answer + Value && _dynamicQuiz.LimitMinAnswer <= _dynamicQuiz.Answer + Value)
                    _dynamicQuiz.Answer += Value;
            }
            else
            {
                    _dynamicQuiz.Answer = Value;
            }
        }
    }
}