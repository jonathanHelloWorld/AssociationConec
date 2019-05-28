using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    public class ButtonDisableButtonsOnClick : ButtonView
    {
        public Button[] btns;

        protected override void OnClick()
        {
            base.OnClick();

            for(int i = 0; i < btns.Length; i++)
            {
                btns[i].interactable = false;
            }
        }
    }
}