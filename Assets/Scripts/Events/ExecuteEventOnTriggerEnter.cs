using UnityEngine;

namespace InterativaSystem.Views.Events
{
    public class ExecuteEventOnTriggerEnter : ExecuteEvent
    {
        public string listenToTag;

        void OnTriggerEnter(Collider col)
        {
            if(col.CompareTag(listenToTag))
            {
                EventStart();
            }
        }

        void OnTriggerStay(Collider col)
        {
            if (col.CompareTag(listenToTag))
            {
                EventRepeat();
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.CompareTag(listenToTag))
            {
                EventEnd();
            }
        }
    }
}
