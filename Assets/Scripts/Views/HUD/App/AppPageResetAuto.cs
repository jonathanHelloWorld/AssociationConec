using System;
using System.Collections;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using UnityEngine;
using Assets.Scripts.Views.HUD;

namespace InterativaSystem.Views.HUD.App
{
    [RequireComponent(typeof(Page.Page))]
    public class AppPageResetAuto : DoOnPageAuto
    {
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void DoSomething()
        {
            base.DoSomething();
			ControllerNewAssociation.Instance.ended = false;
            _bootstrap.ResetApp();
        }
    }
}