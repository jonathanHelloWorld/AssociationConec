using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.DynamicQuiz
{
    public class DynamicQuizAnswerButton : ButtonView
    {
        private DynamicQuizPointBased _dynamicQuiz;

        protected override void OnStart()
        {
            base.OnStart();

            _dynamicQuiz = _controller as DynamicQuizPointBased;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _bt.interactable = false;
            StartCoroutine(Reenable());

            _dynamicQuiz.ReceiveAnswer();
        }

        IEnumerator Reenable()
        {
            yield return new WaitForSeconds(3.2f);
            _bt.interactable = true;
        }

        protected override void ResetView()
        {
            base.ResetView();


            _bt.interactable = true;
        }
    }
}