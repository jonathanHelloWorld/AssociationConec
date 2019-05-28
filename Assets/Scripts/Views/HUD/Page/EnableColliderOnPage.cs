using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;
using Assets.Scripts.Views.HUD;

namespace InterativaSystem.Views.HUD.Page
{
    public class EnableColliderOnPage : DoOnPageAuto
    {
        public Collider[] _collider;

        protected override void OnStart()
        {
            base.OnStart();

            _pagesController.Reset += StopCoroutines;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }

        protected override void DoSomething()
        {
            for(int i = 0; i < _collider.Length; i++)
                _collider[i].enabled = true;
        }
    }
}