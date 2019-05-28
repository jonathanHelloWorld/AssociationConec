using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    [RequireComponent(typeof(Page))]
    public class PageChangeTimeout : GenericView
    {
        [Space]
        public PageButtonOption option;
        [Space]
        public int Especific;
        public PageType Type;

        protected PagesController _pagesController;

        protected int _id;
        protected PageType _type;

        protected bool _routineIsRunning;

        protected override void OnStart()
        {
            _pagesController = _controller as PagesController;

            _routineIsRunning = false;

            var page = GetComponent<Page>();
            _id = page.Id;
            _type = page.Type;

            _timeController.Timeout += CheckPage;

            _pagesController.Reset += StopCoroutines;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }

        void CheckPage()
        {
			UnityEngine.Debug.Log("eaeeeeeeeeeeeeee");
            if (!_routineIsRunning && _pagesController.actualPage == _id)
            {
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
}