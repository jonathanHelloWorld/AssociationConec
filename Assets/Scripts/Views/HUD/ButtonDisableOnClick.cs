using UnityEngine;
using System.Collections;

namespace InterativaSystem.Views.HUD
{
    public class ButtonDisableOnClick : ButtonView
    {
        protected override void OnClick()
        {
            base.OnClick();

            _bt.interactable = false;
        }
    }
}