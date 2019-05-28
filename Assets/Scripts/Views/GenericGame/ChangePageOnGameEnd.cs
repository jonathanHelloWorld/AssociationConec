using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;
using InterativaSystem.Views.HUD.Page;

namespace InterativaSystem.Views.GenericGame
{
    public class ChangePageOnGameEnd : GenericView
    {
        [Space]
        public PageButtonOption option;
        [Space]
        public int Especific;
        public PageType Type;

        private PagesController _pagesController;
        private bool _gameStarted;

        Page page;

        public float delay;

        protected override void OnStart()
        {
            page = GetComponent<Page>();

            _controller.OnGameStart += OnGameStart;
            _controller.OnGameEnd += OnGameEnd;
            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;
        }

        private void OnGameStart()
        {
            _gameStarted = true;
        }

        void OnGameEnd()
        {
            if (!_gameStarted) return;

            if (_pagesController.actualPage == page.Id)
            {
                Debug.Log("Game Ended Next Page");
                StartCoroutine(End());
                _gameStarted = false;
            }
            /*
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
            /**/
        }

        IEnumerator End()
        {
            yield return new WaitForSeconds(delay);

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