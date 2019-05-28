using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Movie
{
    [RequireComponent(typeof(AVProWindowsMediaMovie))]
    public class MoviePlayOnPageQTPro : RawImageView
    {
        [HideInInspector]
        public AVProWindowsMediaMovie Movie;

        public Renderer Renderer;

        private Page.Page _page;
        private PagesController _pagesController;

        private int _id;
        private PageType _type;

        protected override void OnStart()
        {
            base.OnStart();

            if (Renderer != null)
                Renderer.material.mainTexture = Movie.OutputTexture;

            Movie = GetComponent<AVProWindowsMediaMovie>();
            _image.texture = Movie.OutputTexture;


            _page = GetComponent<Page.Page>();

            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;

            _id = _page.Id;
            _type = _page.Type;

            _pagesController.NextPage += CheckPage;
            _pagesController.PreviousPage += CheckPage;
            _pagesController.OpenPage += CheckPage;
            _pagesController.OpenPageType += CheckPage;
        }

        void CheckPage()
        {
            if (_pagesController.actualPage == _id)
                Play();
        }
        void CheckPage(int id)
        {
            if (id == _id)
                Play();
        }
        void CheckPage(PageType type)
        {
            if (type == _type)
                Play();
        }

        void Play()
        {
            Movie.Start();
            Movie.Play();
        }
    }
}