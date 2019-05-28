using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class Page : CanvasGroupView
    {
        public int Id;
        public PageType Type;

        private PagesController _pagesController;

        protected override void OnStart()
        {
            _pagesController = _controller as PagesController;

            _pagesController.NextPage += CheckPage;
            _pagesController.PreviousPage += CheckPage;
            _pagesController.OpenPage += CheckPage;
            _pagesController.OpenPageType += CheckPage;

            _bootstrap.AppStarted += CheckPage;
        }

        void CheckPage()
        {
            if (_pagesController.actualPage == Id)
                Show();
            else
                Hide();
        }
        void CheckPage(int id)
        {
            if (id == Id)
                Show();
            else
                Hide();
        }
        void CheckPage(PageType type)
        {
            if (Type != PageType.Generic && type == Type)
            {
                Show();
                _pagesController.SetActualPage(Id);
            }
            else
                Hide();
        }

        public override void Hide()
        {
            base.Hide();
            /*
            _cv.alpha = 0;
            _cv.blocksRaycasts = false;
            _cv.interactable = false;
            /* */
        }
        public override void Show()
        {
            base.Show();
            /*
            _cv.alpha = 1;
            _cv.blocksRaycasts = true;
            _cv.interactable = true;
             * */
        }
    }
}