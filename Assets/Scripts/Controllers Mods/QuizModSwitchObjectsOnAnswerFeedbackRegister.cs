using InterativaSystem.Controllers;
using InterativaSystem.Views;
using InterativaSystem.Views.HUD.Page;
using UnityEngine;

namespace InterativaSystem.ControllersMods
{
    public class QuizModSwitchObjectsOnAnswerFeedbackRegister : GenericView
    {
        public QuizModSwitchObjectsOnAnswerFeedback script;
        public Page page;
        public GameObject switchFrom;
        public GameObject switchTo;
        public bool switched = false;
        public GameObject teleport;

        protected override void OnStart()
        {
            base.OnStart();

            script.RegisterSwitch(page.Id, switchFrom, switchTo, teleport);
        }
    }
}