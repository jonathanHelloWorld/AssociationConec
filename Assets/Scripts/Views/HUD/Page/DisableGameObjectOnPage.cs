using System.Collections;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class DisableGameObjectOnPage : DoOnPageAuto
    {
        public GameObject[] GameObjects;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void DoSomething()
        {
            for(int i = 0; i < GameObjects.Length; i++)
            {
                GameObjects[i].SetActive(false);
            }
        }
    }
}