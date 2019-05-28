using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.ControllersMods
{
    public class QuizModResetQuestionsData : GenericView
    {
        QuizController _quizController;
        public QuestionsData _questions;

        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _controller as QuizController;
            _quizController.Reset += ResetQuestions;

            _questions = _bootstrap.GetModel(ModelTypes.Questions) as QuestionsData;
        }

        void ResetQuestions()
        {
            _questions.CallReset();
        }
    }
}
