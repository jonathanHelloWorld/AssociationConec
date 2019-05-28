using System.Security.Cryptography.X509Certificates;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Dynamic Quiz Controller")]
    public class DynamicQuizPointBased : GenericController
    {
        //[HideInInspector]
        public int Points;
        public int Answer;

        public event SimpleEvent OnFirstFaseEnd;

        public event BoolEvent OnReceiveAnswer;
        protected SFXController _sfx;
        
        private RegisterController _registration;
        private bool isFirst;

        private TimeController _timeController;
        private PagesController _pagesController;

        private DynamicQuestionData _questionData;

        public bool right;

        public int LimitMaxPoints;
        public int LimitMaxAnswer;
        public int LimitMinAnswer;
        public int SelectedChar = 0;

        public void SelectChar(int i)
        {
            SelectedChar = i;
        }

        private void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.DynamicQuiz;

            CallReset();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;
            _questionData = _bootstrap.GetModel(ModelTypes.DynamicQuestions) as DynamicQuestionData;

            _registration = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;

            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;

            if (_timeController != null)
                _timeController.GameTimeout += GameTimeout;
        }

        private void GameTimeout()
        {
            _pagesController.GoToNextPage();

            if (OnFirstFaseEnd != null) OnFirstFaseEnd();
        }

        public virtual void AddPoints(int value)
        {
            Points += value;
            if (LimitMaxPoints > 0 && Points > LimitMaxPoints)
                Points = LimitMaxPoints;
        }

        protected override void CallReset()
        {
            base.CallReset();
            right = false;
            isFirst = true;
            Points = 0;
            Answer = 0;
        }

        bool CalculateRight(int value)
        {
            return _questionData.CheckAnswer(value);
        }

        public void ReceiveAnswer()
        {
            if (isFirst)
            {
                _registration.AddRegisterValue("Points", Points.ToString(), false);
                _registration.AddRegisterValue("Answer", Answer.ToString(), false);
            }
            if (CalculateRight(Answer))
            {
                DebugLog("Right");

                QuestionRight();

                EndGame();
                if (isFirst)
                    _registration.AddRegisterValue("IsRight", "true", false);
            }
            else
            {
                DebugLog("Wrong");

                QuestionWrong();
                if (isFirst)
                    _registration.AddRegisterValue("IsRight", "false", false);
            }
            isFirst = false;
        }
        public void ReceiveAnswer(int value)
        {
            if (CalculateRight(value))
            {
                DebugLog("Right");
                
                QuestionRight();
            }
            else
            {
                DebugLog("Wrong");

                QuestionWrong();
            }
        }
        protected void QuestionRight()
        {
            _sfx.PlaySound("Right");
            right = true;
            OnReceiveAnswer(true);
        }

        protected void QuestionWrong()
        {
            _sfx.PlaySound("Wrong");
            OnReceiveAnswer(false);
        }
    }
}