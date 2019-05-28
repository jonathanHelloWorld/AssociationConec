using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Network.Server
{
    public class NetworkQuizSendModelQuestion : ButtonView
    {
        private QuestionsData _questions;

        public bool SendToPassive;

        protected override void OnStart()
        {
            _questions = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
        }
        protected override void OnClick()
        {
#if HAS_SERVER
            base.OnClick();

            if (SendToPassive)
            {
                UnityEngine.Debug.LogError("Not implemented to passive Sending to All");
                //TODO Yuri: Send only to passive Actual questions From Server
                _questions.SendActualQuestion();
            }
            else
            {
                _questions.SendActualQuestion();
            }
#endif
        }
    }
}