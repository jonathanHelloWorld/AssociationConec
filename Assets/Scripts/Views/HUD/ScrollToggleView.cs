using InterativaSystem.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Scrollbar))]
    [RequireComponent(typeof(Image))]
    public class ScrollToggleView : GenericView, ISelectHandler
    {
        protected Scrollbar _scroll;
        protected Image _bg;
        protected override void OnAwake()
        {
            _scroll = GetComponent<Scrollbar>();
            _bg = GetComponent<Image>();
            _scroll.onValueChanged.AddListener(OnValueChanged);
        }
        public void OnSelect(BaseEventData eventData)
        {
            OnSelected();
        }

        protected virtual void OnValueChanged(float value) { }
        protected virtual void OnSelected() { }
    }
}