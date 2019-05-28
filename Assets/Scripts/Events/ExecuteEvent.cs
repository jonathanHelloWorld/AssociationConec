using InterativaSystem.Views;
using UnityEngine.Events;
using InterativaSystem.Controllers;

namespace InterativaSystem.Views.Events
{
    public class ExecuteEvent : GenericEvent
    {
        public event GenericController.SimpleEvent DoOnEventStart, DoOnEventRepeat, DoOnEventEnd;

        protected virtual void EventStart()
        {
            if (DoOnEventStart != null)
                DoOnEventStart();
        }

        protected virtual void EventRepeat()
        {
            if (DoOnEventRepeat != null)
                DoOnEventRepeat();
        }

        protected virtual void EventEnd()
        {
            if (DoOnEventEnd != null)
                DoOnEventEnd();
        }
    }
}
