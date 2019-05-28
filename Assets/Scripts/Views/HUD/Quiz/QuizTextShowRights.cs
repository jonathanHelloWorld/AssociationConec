using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizTextShowRights : TextView
    {
        private QuizController quizController;

        public bool ShowTotal;
        public bool TEMPQuiz16133; //TODO Yuri: Eliminar TEMPQuiz16133

        protected override void OnStart()
        {
            base.OnStart();

            quizController = _bootstrap.GetController(ControllerTypes.Quiz) as QuizController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (ShowTotal)
                _tx.text = quizController.QuestionsRight.ToString("00") + "/" + quizController.QuestionLimit.ToString("00");
            else if(TEMPQuiz16133)
                _tx.text = (quizController.QuestionsRight*100).ToString("000");
            else
                _tx.text = quizController.QuestionsRight.ToString("00");
        }
    }
}