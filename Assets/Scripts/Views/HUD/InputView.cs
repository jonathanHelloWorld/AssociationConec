using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(InputField))]
    public class InputView : GenericView, ISelectHandler
    {
        [HideInInspector]
        public InputField input;

        protected override void OnAwake()
        {
            input = GetComponent<InputField>();
            input.onEndEdit.AddListener(EndEdit);
#if UNITY_5_3
            input.onValueChanged.AddListener(ValueChanged);
#else
            input.onValueChanged.AddListener(ValueChanged);
#endif
        }

        protected virtual void EndEdit(string value) { }
        protected virtual void ValueChanged(string value) { }
        public virtual void OnSelect(BaseEventData eventData) { }
    }
}