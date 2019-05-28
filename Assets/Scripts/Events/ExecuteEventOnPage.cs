using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Page;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Views.Events
{
    [RequireComponent(typeof(Page))]
    public class ExecuteEventOnPage : ExecuteEvent
    {
        PagesController _pagesController;
        Page page;

        int pageId;
        PageType pageType;

        protected override void OnStart()
        {
            base.OnStart();

            page = GetComponent<Page>();
            pageId = page.Id;
            pageType = page.Type;

            _pagesController = _controller as PagesController;
            _pagesController.NextPage += CheckPage;
            _pagesController.PreviousPage += CheckPage;
            _pagesController.OpenPage += CheckPage;
            _pagesController.OpenPageType += CheckPage;
        }

        void CheckPage()
        {
            if (_pagesController.actualPage == pageId)
                EventStart();
        }

        void CheckPage(int id)
        {
            if (id == pageId)
                EventStart();
        }

        void CheckPage(PageType type)
        {
            if (type == pageType)
                EventStart();
        }
    }
}