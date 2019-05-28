using System.Collections;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class ToggleGameObjectOnPage : DoOnPageAuto
    {
        public GameObject[] GameObjects;
        public bool toggle = false;

        protected override void OnStart()
        {
            base.OnStart();

            //NotThisPage();
        }

        protected override void DoSomething()
        {
            for(int i = 0; i < GameObjects.Length; i++)
            {
                GameObjects[i].SetActive(true);
            }
        }

        protected override void NotThisPage()
        {
            for (int i = 0; i < GameObjects.Length; i++)
            {
                GameObjects[i].SetActive(false);
            }
        }
    }
}