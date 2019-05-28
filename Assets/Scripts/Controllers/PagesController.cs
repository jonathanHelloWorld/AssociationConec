using System;
using System.Linq;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    public enum PageButtonOption
    {
        Next = 0,
        Previous = 1,
        Especific = 2,
        Type = 4
    }
    public enum PageType
    {
        Generic = 0,
        Home = 1,
        End = 10,
        Registry = 2,
        Help = 3,
        Game = 5,
        Publicity = 8
    }
    [AddComponentMenu("ModularSystem/Controllers/Pages Controller")]
    public class PagesController : GenericController
    {
        public delegate void PageEvent(PageType value);

        public event SimpleEvent NextPage , PreviousPage;
        public event IntEvent OpenPage;
        public event PageEvent OpenPageType;

        [HideInInspector]
        public int actualPage;

        #region Initialization
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Page;
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void GetReferences()
        {
            base.GetReferences();
        }
        #endregion

        #region Page Transitions Methods
        private void GoHome()
        {
            OpenPageByType(PageType.Home);
        }
        public void GoToNextPage()
        {
            actualPage++;
            if (NextPage != null) NextPage();
        }
        public void GoToPreviousPage()
        {
            actualPage--;
            if (PreviousPage != null) PreviousPage();
        }
        public void OpenPageById(int page)
        {
            if (OpenPage != null) OpenPage(page);
            actualPage = page;
        }
        public void OpenPageByType(PageType page)
        {
            if (OpenPageType != null) OpenPageType(page);
        }
        public void SetActualPage(int id)
        {
            actualPage = id;
        }
        #endregion

        #region Networking specific methods
#if HAS_SERVER
        //Methods just convert and callback the original method
        public void NetworkGoToNextPage(string json)
        {
            if (_isServer) return;

            GoToNextPage();
        }
        public void NetworkGoToPreviousPage(string json)
        {
            if (_isServer) return;

            GoToPreviousPage();
        }
        public void NetworkOpenPageById(string json)
        {
            if (_isServer) return;

            var page = JsonConvert.DeserializeObject<int>(json);
            OpenPageById(page);
        }
        public void NetworkOpenPageByType(string json)
        {
            if (_isServer) return;

            var page = JsonConvert.DeserializeObject<PageType>(json);
            OpenPageByType(page);
        }
#endif
        #endregion
    }
}