using UnityEngine;
using UnityEngine.EventSystems;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(EventSystem))]
    public class EventSystemView : GenericView
    {
        private EventSystem _eventSystem;

        protected override void OnStart()
        {
            base.OnStart();

            _eventSystem = GetComponent<EventSystem>();
        }

        protected virtual void Deselect()
        {
            _eventSystem.SetSelectedGameObject(null);
        }

        protected virtual void Select(GameObject obj)
        {
            _eventSystem.SetSelectedGameObject(obj);
        }
    }
}