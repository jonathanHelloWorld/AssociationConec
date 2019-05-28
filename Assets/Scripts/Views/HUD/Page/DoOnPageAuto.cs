using System.Collections;
using InterativaSystem.Controllers;
using UnityEngine;
using InterativaSystem.Views.HUD.Page;
using InterativaSystem.Views;
using InterativaSystem;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Page.Page))]
    public class DoOnPageAuto : GenericView
    {
        protected PagesController _pagesController;

        public float TransitionTime;

        protected int _id;
        protected PageType _type;
        protected bool _routineIsRunning;

        protected override void OnStart()
        {
            _pagesController = _controller as PagesController;

            var page = GetComponent<Page.Page>();
            _id = page.Id;
            _type = page.Type;

            _pagesController.NextPage += CheckPage;
            _pagesController.PreviousPage += CheckPage;
            _pagesController.OpenPage += CheckPage;
            _pagesController.OpenPageType += CheckPage;
        }

        private void StopCoroutines()
        {
            StopAllCoroutines();
        }

        protected virtual void CheckPage()
        {
            if (_pagesController.actualPage == _id)
                Execute();
            else
                NotThisPage();
        }
        protected virtual void CheckPage(int id)
        {
            if (id == _id)
                Execute();
            else
                NotThisPage();
        }
        protected virtual void CheckPage(PageType type)
        {
            if (type == _type)
                Execute();
            else
                NotThisPage();
        }

        protected void Execute()
        {
            if (!_routineIsRunning)
                StartCoroutine(Wait());
        }

        protected virtual void NotThisPage() { }

        protected IEnumerator Wait()
        {
            _routineIsRunning = true;
            yield return new WaitForSeconds(TransitionTime);
            DoSomething();

            _routineIsRunning = false;
        }

        protected virtual void DoSomething() {} 
    }
}