using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Views;
using UnityEngine;

namespace InterativaSystem.Views.Events.Functions
{
    public class FuncPageChange : GenericEvent
    {
        [Space(10f)]
        public int PageTo;

        private PagesController _pagesController;

        protected override void OnStart()
        {
            base.OnStart();

            _pagesController = _controller as PagesController;
        }

        protected override void RunEvent()
        {
            ChangePage();
        }

        void ChangePage()
        {
            _pagesController.OpenPageById(PageTo);
        }
    }
}
