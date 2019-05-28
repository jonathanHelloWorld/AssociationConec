using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using InterativaSystem.Controllers;
using DG.Tweening;

namespace Interativa.Views.CandyGame
{
    public class CandyPopUp : GenericView
    {
        protected override void OnStart()
        {
            base.OnStart();

            ((CandyController)_controller).NoMovementsPopUp += Show;

            gameObject.SetActive(false);
        }

        void Show(float duration)
        {
            gameObject.SetActive(true);

            transform.SetAsLastSibling();
        }

        void KillMe()
        {
            gameObject.SetActive(false);
        }
    }
}