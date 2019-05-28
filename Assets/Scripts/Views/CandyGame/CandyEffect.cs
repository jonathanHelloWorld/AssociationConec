using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using InterativaSystem.Controllers;

namespace Interativa.Views.CandyGame
{
    public class CandyEffect : GenericView
    {
        public float duration;

        protected override void OnStart()
        {
            base.OnStart();

            ((CandyController)_controller).CreateSpecial += Show;

            Prepare();
        }

        public void Prepare()
        {
            gameObject.SetActive(false);
        }

        public void Show(Vector3 position)
        {
            transform.gameObject.SetActive(true);
            transform.localPosition = position;

            Invoke("KillMe", duration);
        }

        void KillMe()
        {
            gameObject.SetActive(false);
        }
    }
}