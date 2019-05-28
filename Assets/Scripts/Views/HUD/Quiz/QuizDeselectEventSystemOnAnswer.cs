using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizDeselectEventSystemOnAnswer : EventSystemView
    {
        protected QuizController _quizController;
        public float Delay;
        protected override void OnStart()
        {
            base.OnStart();
            
            _quizController = _controller as QuizController;
            _quizController.OnReceiveAnswer += WaitForDeselect;
        }

        private void WaitForDeselect(int value)
        {
            Invoke("Deselect", Delay);
        }
    }
}