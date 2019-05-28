using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class PageChangeButton : ButtonView
    {
        [Space]
        public PageButtonOption option;
        [Space]
        public int Especific;
        public PageType Type;

        private PagesController _pagesController;

        protected override void OnStart()
        {
            _pagesController = _controller as PagesController;
        }

        protected override void OnClick()
        {
            base.OnClick();

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