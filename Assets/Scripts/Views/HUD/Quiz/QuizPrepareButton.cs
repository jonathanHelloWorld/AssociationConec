using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Quiz
{
    public class QuizPrepareButton : ButtonView
    {
        public event GenericController.IntEvent OnSelection;

        public bool HasTypes;
        public bool BlockOnReset;
        [SerializeField] int _Type;
        public int Type
        {
            get { return _Type; }
            set
            {
                _Type = value;

                if (OnSelection != null)
                    OnSelection(value);

                _bt.interactable = true;
            }
        }

        private QuizController _quizController;

        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;

            if (HasTypes && BlockOnReset)
            {
                _quizController.Reset += () =>
                {
                    _bt.interactable = false;
                };
            }
        }


        protected override void OnClick()
        {
            base.OnClick();

            if (HasTypes)
                _quizController.PrepareGame(Type);
            else
                _quizController.PrepareGame();
        }
    }
}