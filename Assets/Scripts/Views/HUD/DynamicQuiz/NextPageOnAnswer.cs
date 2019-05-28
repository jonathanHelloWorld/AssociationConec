using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.DynamicQuiz
{
    public class NextPageOnAnswer : GenericView
    {

        [Header("Page Right")]
        [Space]
        public float TransitionTime = 5;
        [Space]
        public PageButtonOption Option;
        [Space]
        public int Especific;
        public PageType Type;

        [Header("Page Wrong")]
        [Space]
        public float TransitionTimeW = 5;
        [Space]
        public PageButtonOption OptionW;
        [Space]
        public int EspecificW;
        public PageType TypeW;


        private PagesController _pagesController;
        private DynamicQuizPointBased _dynamicQuiz;

        private int _id;
        private bool _routineIsRunning;

        protected override void OnStart()
        {
            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;
            _dynamicQuiz = _controller as DynamicQuizPointBased;

            var page = GetComponent<Page.Page>();
            _id = page.Id;

            _dynamicQuiz.OnReceiveAnswer += PassPage;

            _pagesController.Reset += StopCoroutines;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }

        void PassPage(bool right)
        {
            if (_id != _pagesController.actualPage) return;

            if(!_routineIsRunning)
                StartCoroutine(Wait(right));
        }
        IEnumerator Wait(bool right)
        {
            _routineIsRunning = true;
            if(right)
                yield return new WaitForSeconds(TransitionTime);
            else
                yield return new WaitForSeconds(TransitionTimeW);

            if (right)
            {
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
                        if (Type == PageType.Generic)
                            break;
                        _pagesController.OpenPageByType(Type);
                        break;
                }
            }
            else
            {
                switch (OptionW)
                {
                    case PageButtonOption.Next:
                        _pagesController.GoToNextPage();
                        break;
                    case PageButtonOption.Previous:
                        _pagesController.GoToPreviousPage();
                        break;
                    case PageButtonOption.Especific:
                        _pagesController.OpenPageById(EspecificW);
                        break;
                    case PageButtonOption.Type:
                        if (Type == PageType.Generic)
                            break;
                        _pagesController.OpenPageByType(TypeW);
                        break;
                }
            }

            _routineIsRunning = false;
        }
    }
}