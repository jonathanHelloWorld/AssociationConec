using InterativaSystem.Controllers;
using InterativaSystem.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InterativaSystem.ControllersMods
{
    public class QuizModSwitchObjectsOnAnswerFeedback : GenericView
    {
        PagesController _pagesController;
        QuizController _quizController;

        public Dictionary<int, ObjectSwitch> switchs;

        protected override void OnAwake()
        {
            base.OnAwake();

            switchs = new Dictionary<int, ObjectSwitch>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;

            _quizController = _controller as QuizController;
            _quizController.OnAnswerFeedback += OnAnswerFeedback;
            _quizController.Reset += OnReset;
        }

        public void RegisterSwitch(int id, GameObject switchFrom, GameObject switchTo, GameObject teleport)
        {
            switchs.Add(id, new ObjectSwitch(switchFrom, switchTo, teleport));
        }

        void OnAnswerFeedback(bool _b)
        {
            switchs[_pagesController.actualPage].switchFrom.SetActive(false);
            switchs[_pagesController.actualPage].switchTo.SetActive(true);
            switchs[_pagesController.actualPage].switched = true;
        }

        void OnReset()
        {
            foreach(ObjectSwitch obj in switchs.Values)
            {
                obj.switchFrom.SetActive(true);
                obj.switchTo.SetActive(false);
                obj.switched = false;
            }
        }
    }

    [System.Serializable]
    public class ObjectSwitch
    {
        public GameObject switchFrom;
        public GameObject switchTo;
        public bool switched = false;
        public GameObject teleport;

        public ObjectSwitch(GameObject _sf, GameObject _st, GameObject _tel)
        {
            switchFrom = _sf;
            switchTo = _st;
            teleport = _tel;
        }
    }
}