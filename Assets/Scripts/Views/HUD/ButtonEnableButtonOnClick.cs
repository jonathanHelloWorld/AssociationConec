using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    public class ButtonEnableButtonOnClick : ButtonView
    {
        public Button targetBtn;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnClick()
        {
            base.OnClick();

            targetBtn.interactable = true;
        }
    }
}