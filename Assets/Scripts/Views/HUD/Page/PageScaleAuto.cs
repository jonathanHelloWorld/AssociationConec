using DG.Tweening;
using InterativaSystem.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class PageScaleAuto : DoOnPageAuto
    {
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void DoSomething()
        {
            transform.DOScaleY(0f, 0.25f).From().Play();
            transform.DOScaleX(0f, 0.25f).From().Play();
        }
    }
}
