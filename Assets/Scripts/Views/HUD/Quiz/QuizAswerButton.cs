using DG.Tweening;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Quiz
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(CanvasGroup))]
    public class QuizAswerButton : ButtonView
    {
        protected QuizController _quizController;
        protected QuestionsData _questionsData;

        protected CanvasGroup _cv;

        protected Image _image;

        public bool WaitShowFeedback;

        public int Id;
        public Color Right = Color.green;
        public Color Wrong = Color.red;
        public Color Selection = Color.white;
        public Sprite RightSprite;
        public Sprite WrongSprite;
        public Sprite SelectionSprite;
        private Sprite _iniSprite;
        private Color _iniColor;

        public bool HideOnPrepared;

        private bool clicked;

        protected override void OnAwake()
        {
            base.OnAwake();

            _image = GetComponent<Image>();
            _iniColor = _image.color;
            _iniSprite = _image.sprite;
        }
        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.OnReceiveAnswer += FeedBack;
            _quizController.OnQuestionDone += Reset;
            _quizController.OnClick += Deselect;

            _cv = GetComponent<CanvasGroup>();

            _questionsData = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
            _questionsData.OnNewQuestionReady += SetButton;


            if (HideOnPrepared)
            {
                _quizController.OnGamePrepare += Hide;
                _quizController.Reset += Hide;
                //_quizController.OnGameStart += Show;
                Hide();
            }
        }


        protected override void ResetView()
        {
            base.ResetView();

            _cv.interactable = false;

            _image.color = _iniColor;
            _image.sprite = _iniSprite;
        }

        private void SetButton()
        {
            if (Id >= _questionsData.GetQuestion().alternatives.Count)
                Hide();
            else
                Show();
        }
        void Show()
        {
            gameObject.SetActive(true);
            _cv.DOFade(1, 0.4f).Play();
            _cv.interactable = true;
            _cv.blocksRaycasts = true;
        }
        void Hide()
        {
            _cv.DOFade(0, 0.4f).OnComplete(() => gameObject.SetActive(false)).Play();
            _cv.interactable = false;
            _cv.blocksRaycasts = false;
        }
        void Reset()
        {
            if(_cv == null) return;

            _cv.interactable = true;

            _image.color = _iniColor;
            _image.sprite = _iniSprite;
        }
        private void Deselect()
        {
            if (!WaitShowFeedback) return;

            _image.color = _iniColor;
            _image.sprite = _iniSprite;
            clicked = false;
        }
        protected override void OnClick()
        {
            base.OnClick();

            clicked = true;

            _quizController.ReceiveAnswer(Id);
            
            if (!WaitShowFeedback) return;

            if (RightSprite == null)
                _image.color = Selection;
            else
                _image.sprite = SelectionSprite;
        }
        private void FeedBack(int value)
        {
            _cv.interactable = false;

            if (RightSprite == null)
            {
                if (value == Id)
                    _image.color = Right;
                else if (clicked)
                    _image.color = Wrong;
            }
            else
            {
                if (value == Id)
                    _image.sprite =  RightSprite;
                else if (clicked)
                    _image.sprite = WrongSprite;
            }

            clicked = false;
        }
    }
}