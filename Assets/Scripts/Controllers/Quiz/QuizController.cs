using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    public enum QuizScore { ByQuestion, Score }
    public enum ProductType
    {
        FaturaPremiada = 0,
        LarSeguro = 1,
        SeguracoPremiado = 2,
        SorteGrande = 3
    }

    [AddComponentMenu("ModularSystem/Controllers/Quiz Controller")]
    public class QuizController : GenericController
    {
        protected QuestionsData _questions;
        protected SFXController _sfx;
        protected ScoreController _score;

        public event SimpleEvent OnQuestionDone, OnClick;
        public event IntEvent OnReceiveAnswer, OnRightAnswer;
        public event BoolEvent OnAnswerFeedback;


        protected TimeController _timeController;

        public bool SaveToRegistry;
        protected RegisterController _registry;

        public string NameRegistry = "Nome";

        public bool ResetTimeOnAnswer;

        public float WaitTime = 1;

        public int CutOffMark;

        [HideInInspector]
        public int QuestionsRight;

        protected Dictionary<int, int> _answers;
        protected Dictionary<int, float> _times;
        
        public bool WaitShowFeedBack;

        [HideInInspector]
        public int _questionCount;
        public int QuestionLimit;
        protected bool _hasTypes;
        public bool HasTypes
        {
            get { return _hasTypes; }
            set { _hasTypes = value; }
        }

        protected int _type;
        public int type
        {
            get { return _type; }
            set { _type = value; }
        }
        [HideInInspector]
        public float passed;

        public QuizScore ScoreType;
        public int QuestionScore;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Quiz;

            QuestionsRight = 0;
            _questionCount = 0;
        }

        protected override void CallReset()
        {
            base.CallReset();

            QuestionsRight = 0;
            _questionCount = 0;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _score = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;

            _questions = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
            _questions.OnNewQuestionReady += QuestionDone;
            _questions.OnQuestionsOver += NoMoreQuestions;

            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            if (_timeController != null)
                _timeController.GameTimeout += TimeOut;

            if (SaveToRegistry)
                _registry = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;

            _answers = new Dictionary<int, int>();
        }

        protected virtual void TimeOut()
        {
#if HAS_SERVER
            if (WaitShowFeedBack) return;
#endif
            if (!IsGameRunning) return;

            if (ResetTimeOnAnswer)
            {
                ReceiveAnswer(-1);
            }
            else
                _bootstrap.EndGame(this);
        }

        public override void EndGame()
        {
            base.EndGame();

            if (SaveToRegistry)
            {
                _registry.AddRegisterValue("QuizType", type.ToString(), false);
                _registry.AddRegisterValue("QuizAnswers", JsonConvert.SerializeObject(_answers), false);

                if (_score != null)
                {
                    if(_timeController != null)
                        passed = _times.Sum(x => x.Value);

                    if (WaitShowFeedBack){
                        passed = _times.Sum(x => x.Value);
                        return;
                    }

                    if (ScoreType == QuizScore.ByQuestion)
                    {
                        if(HasTypes)
                            _score.AddScore(QuestionsRight, passed, type);
                        else
                            _score.AddScore(QuestionsRight, passed);
                    }
                }
            }
            
            if (WaitShowFeedBack) return;

            //SendrightQuestionFeedBack();
        }
        
        public override void StartGame()
        {
            base.StartGame();

            _answers = new Dictionary<int, int>();
            _times = new Dictionary<int, float>();
        }

        protected void QuestionDone()
        {
#if HAS_SERVER
            if (WaitShowFeedBack && _timeController != null && !_isServer)
            {
                _timeController.StartGameTimer();
            }
#else
            if (_timeController != null)
            {
                _timeController.StartGameTimer();
            }
#endif
            if (OnQuestionDone != null) OnQuestionDone();
        }

        protected void NoMoreQuestions()
        {
            DebugLog("NoMoreQuestions");
            _bootstrap.EndGame(this);
        }

        public override void PrepareGame()
        {
            base.PrepareGame();

            _hasTypes = false;

#if HAS_SERVER
            if(WaitShowFeedBack) return;
#endif

            CallNextQuestion();
        }
        public void PrepareGame(int t)
        {
            base.PrepareGame();

            _hasTypes   = true;
            _type       = t;

            CallNextQuestion();
        }

        public void CallNextQuestion()
        {
            if (_hasTypes)
                _questions.NextQuestion(_type);
            else
                _questions.NextQuestion();
        }

        protected void QuestionRight()
        {
            _sfx.PlaySound("Right");
            QuestionsRight++;

            if (OnAnswerFeedback != null) OnAnswerFeedback(true);

            if (OnRightAnswer != null)
                OnRightAnswer(QuestionsRight);

            //if (!WaitShowFeedBack) return;

            if (_score != null)
            {
                passed = _times.Sum(x => x.Value);

                if (ScoreType == QuizScore.Score)
                    _score.AddScore(QuestionScore, passed);
            }
        }

        protected void QuestionWrong()
        {
            _sfx.PlaySound("Wrong");

            if (OnAnswerFeedback != null) OnAnswerFeedback(false);
            
            //if (!WaitShowFeedBack) return;

            if (_score != null)
            {
                passed = _times.Sum(x => x.Value);

                if (ScoreType == QuizScore.Score)
                    _score.AddScore(0, passed);
            }
        }

        public void ReceiveAnswer(int id)
        {
            if (OnClick != null) OnClick();

            if (_timeController != null)
            {
                _timeController.Paused = true;

                if (!_times.ContainsKey(_questions.ActualQuestion))
                    _times.Add(_questions.ActualQuestion, _timeController.TimeSinceGameStart);
                //else
                //_times[_questions.ActualQuestion] = _timeController.GameTimeLimit;
            }
            
            if (WaitShowFeedBack)
            {
                if (_answers.ContainsKey(_questions.ActualQuestion))
                {
                    _answers[_questions.ActualQuestion] = id;
                }

                if (_answers.Count > _questionCount) return;

                if (!_answers.ContainsKey(_questions.ActualQuestion))
                    _answers.Add(_questions.ActualQuestion, id);
                else
                    _answers[_questions.ActualQuestion] = id;
                
                if (ResetTimeOnAnswer && id == -1)
                    SendrightQuestionFeedBack();

                return;
            }

            _questionCount++;

            if (_questions.CheckAnswer(id))
                QuestionRight();
            else
                QuestionWrong();

            if (!_answers.ContainsKey(_questions.ActualQuestion))
                _answers.Add(_questions.ActualQuestion, id);
            else
                _answers[_questions.ActualQuestion] = id;
            
            SendrightQuestionFeedBack();
        }

        public void AddFakeAnswer()
        {
            _questionCount++;
            if (_questionCount >= QuestionLimit)
                _bootstrap.EndGame(this);
        }

        public void SendrightQuestionFeedBack()
        {
            if (HasTypes)
            {
                if (OnReceiveAnswer != null)
                {
                    List<Question> list = _questions.Questions.FindAll(x => x.type == type);

                    OnReceiveAnswer(list.Find(x => x.id == _questions.ActualQuestion).alternatives.Find(x => x.isRight).id);
                }
            }
            else
            {
                if (OnReceiveAnswer != null)
                    OnReceiveAnswer(_questions.Questions[_questions.ActualQuestion].alternatives.FindIndex(x => x.isRight));

                /*if (!WaitShowFeedBack)
                {
                    _bootstrap.EndGame(this);
                    return;
                }*/
            }

            if (_answers.Count <= _questionCount)
            {
                if (!_answers.ContainsKey(_questions.ActualQuestion))
                    _answers.Add(_questions.ActualQuestion, -1);
                //else
                    //_answers[_questions.ActualQuestion] = -1;
            }

            if (_timeController != null)
            {
                if (!_times.ContainsKey(_questions.ActualQuestion))
                    _times.Add(_questions.ActualQuestion, _timeController.TimeSinceGameStart);
                //else
                //_times[_questions.ActualQuestion] = _timeController.GameTimeLimit;
            }

            //_questionCount++;

            /*if (ResetTimeOnAnswer)
                _timeController.StartGameTimer();*/

            /*if (_questions.CheckAnswer(_answers[_questions.ActualQuestion]))
                QuestionRight();
            else
                QuestionWrong();*/

#if HAS_SERVER
#else
            if (_questionCount < QuestionLimit)
                StartCoroutine(CallNext());
            else
                _bootstrap.EndGame(this);
#endif
        }

        protected IEnumerator CallNext()
        {
            yield return new WaitForSeconds(WaitTime);
            CallNextQuestion();
        }

        public void EndQuestion()
        {
#if HAS_SERVER
            if (WaitShowFeedBack)
            {
                _bootstrap.EndGame(this);
                return;
            }
#endif
            SendrightQuestionFeedBack();
            _bootstrap.EndGame(this);
        }

        public virtual void OpenUDPVote() { }
        public virtual void CloseUDPVote() { }

#if HAS_SERVER
        public void NetworkShowRightQuestion(string json)
        {
            SendrightQuestionFeedBack();
        }
        public void NetworkEndQuestion(string json)
        {
            EndQuestion();
        }
        public virtual void NetworkCloseUDPVote(string json) { }
#endif
    }
}