using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    [RequireComponent(typeof(Page))]
    public class GamePageNextOnEnd : GenericView
    {
        [Space]
        public float TransitionTime = 5;
        [Space]
        public PageButtonOption Option;
        [Space]
        public int Especific;
        public PageType Type;

        private PagesController _pagesController;

        private int _id;
        private bool _routineIsRunning;

        protected override void OnStart()
        {
            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;

            var page = GetComponent<Page>();
            _id = page.Id;

            _controller.OnGameEnd += PassPage;
            _pagesController.Reset += StopCoroutines;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }

        void PassPage()
        {
            if (_id != _pagesController.actualPage) return;

            //UnityEngine.Debug.Log("PassPage");
            if(!_routineIsRunning)
                StartCoroutine("Wait");
        }

        IEnumerator Wait()
        {
            _routineIsRunning = true;
            yield return new WaitForSeconds(TransitionTime);

            switch (Option)
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
                    if(Type == PageType.Generic)
                        break;
                    _pagesController.OpenPageByType(Type);
                    break;
            }

            _routineIsRunning = false;
        }
    }
}