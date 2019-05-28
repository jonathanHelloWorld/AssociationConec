using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class PageOnTimeout : GenericView
    {
        [Space]
        public float TransitionTime = 5;
        [Space]
        public PageButtonOption option;
        [Space]
        public int Especific;
        public PageType Type;

        private PagesController _pagesController;
        
        protected override void OnStart()
        {
            base.OnStart();

            _timeController.Timeout += GoToPage;
            _pagesController = _controller as PagesController;
        }

        void GoToPage()
        {
            StartCoroutine("Wait");
        }
        IEnumerator Wait()
        {
            UnityEngine.Debug.Log("Next Page " + gameObject);

            yield return new WaitForSeconds(TransitionTime);

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