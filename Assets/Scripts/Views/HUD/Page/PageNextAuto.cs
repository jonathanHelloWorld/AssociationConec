using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;
using Assets.Scripts.Views.HUD;

namespace InterativaSystem.Views.HUD.Page
{
    [RequireComponent(typeof(Page))]
    public class PageNextAuto : DoOnPageAuto
    {
        [Space]
        public PageButtonOption option;
        [Space]
        public int Especific;
        public PageType Type;

        private void StopCoroutines()
        {
            StopAllCoroutines();

            _routineIsRunning = false;
        }

        protected override void CheckPage()
        {
            if (!_routineIsRunning && _pagesController.actualPage == _id)
                StartCoroutine(Wait());
            else if(_routineIsRunning && _pagesController.actualPage != _id)
                StopAllCoroutines();
            else
            {
                _routineIsRunning = false;
            }
        }
        protected override void CheckPage(int id)
        {
            if (!_routineIsRunning && id == _id)
                StartCoroutine(Wait());
            else if (_routineIsRunning && id != _id)
                StopAllCoroutines();
        }
        protected override void CheckPage(PageType type)
        {
            if (!_routineIsRunning && Type != PageType.Generic && type == _type)
                StartCoroutine(Wait());
            else if (_routineIsRunning && Type != PageType.Generic && type != _type)
                StopAllCoroutines();
        }

        protected override void DoSomething()
        {
            UnityEngine.Debug.Log("Next Page " + gameObject);

            switch (option)
            {
                case PageButtonOption.Next:
                    _pagesController.GoToNextPage();
                    break;
                case PageButtonOption.Previous:
                    _pagesController.GoToPreviousPage();
                    break;
                case PageButtonOption.Especific:
                    _pagesController.OpenPageById(Especific);
                    break;
                case PageButtonOption.Type:
                    _pagesController.OpenPageByType(Type);
                    break;
            }
        }
    }
}