using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    [RequireComponent(typeof(Page))]
    public class WebCamOnPageStart : GenericView
    {
        private PagesController _pagesController;
        private WebCamController _webCamController;

        private int _id;
        private PageType _type;

        protected override void OnStart()
        {
            _webCamController = _bootstrap.GetController(ControllerTypes.WebCam) as WebCamController;
            _pagesController = _controller as PagesController;

            var page = GetComponent<Page>();
            _id = page.Id;
            _type = page.Type;

            _pagesController.NextPage += CheckPage;
            _pagesController.PreviousPage += CheckPage;
            _pagesController.OpenPage += CheckPage;
            _pagesController.OpenPageType += CheckPage;

            _pagesController.Reset += StopCoroutines;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }

        void CheckPage()
        {
            if (_pagesController.actualPage == _id)
                _webCamController.StartWebCam();
        }
        void CheckPage(int id)
        {
            if (id == _id)
                _webCamController.StartWebCam();
        }
        void CheckPage(PageType type)
        {
            if (_type != PageType.Generic && type == _type)
                _webCamController.StartWebCam();
        }
    }
}