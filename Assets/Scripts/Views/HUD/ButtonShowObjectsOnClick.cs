using UnityEngine;
using System.Collections;

namespace InterativaSystem.Views.HUD
{
    public class ButtonShowObjectsOnClick : ButtonView
    {
        public GameObject[] objs;

        protected override void OnClick()
        {
            base.OnClick();

            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(true);
            }
        }
    }
}