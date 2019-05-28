using InterativaSystem.Views;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace InterativaSystem.Views.Events
{
    public class GenericEvent : GenericView
    {
        public ExecuteEvent ExecuteOnStart, ExecuteOnRepeat, ExecuteOnEnd;
        public float delay = 0f;

        protected override void OnStart()
        {
            base.OnStart();

            if(ExecuteOnStart != null)
                ExecuteOnStart.DoOnEventStart += OnEventStart;

            if (ExecuteOnRepeat != null)
                ExecuteOnRepeat.DoOnEventRepeat += OnEventRepeat;

            if (ExecuteOnEnd != null)
                ExecuteOnEnd.DoOnEventEnd += OnEventEnd;
        }

        protected virtual void RunEvent() { }
        protected IEnumerator _RunEvent() { yield return new WaitForSeconds(delay); RunEvent(); }

        protected virtual void OnEventStart() { StartCoroutine(_RunEvent()); }
        protected virtual void OnEventRepeat() { StartCoroutine(_RunEvent()); }
        protected virtual void OnEventEnd() { StartCoroutine(_RunEvent()); }
    }
}