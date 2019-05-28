using InterativaSystem.Controllers;
using InterativaSystem.Views.EnviromentComponents;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Question
{
    public class QuestionAssociationAlternativeText : QuestionAlternativeText
    {
        private AssociationController _associationController;
        
        [Header("Id is get from parent")]
        public AssociationObject Parent;

        [HideInInspector]
        public int QuestionId;
        [HideInInspector]
        public int ReferenceId;

        protected override void OnAwake()
        {
            base.OnAwake();

            ReferenceId = Parent.QuestionId;

            if (Parent.IsStatic)
                this.enabled = false;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;
            _associationController.OnGamePrepare += GetId;

            GetId();
        }

        private void GetId()
        {
            ReferenceId = Parent.QuestionId;
            QuestionId = _associationController.GetDefinedQuestion();

            SetText();
        }

        protected override void SetText()
        {
            var question = _questionsData.Questions[QuestionId];

            _tx.text = question.alternatives[ReferenceId].title;
        }
    }
}