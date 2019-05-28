using System.Linq;
using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.DynamicQuiz
{
    public class TextBySelectedChar : TextView
    {
        public string[] text;

        private DynamicQuizPointBased _dynamicQuiz;
        protected override void OnStart()
        {
            base.OnStart();

            _dynamicQuiz = _controller as DynamicQuizPointBased;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if(_dynamicQuiz.SelectedChar < text.Length)
                _tx.text = text[_dynamicQuiz.SelectedChar];
        }
    }
}