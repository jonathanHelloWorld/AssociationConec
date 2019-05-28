using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterativaSystem.Views.Others.Settings
{
    [RequireComponent(typeof(InputField))]
    public class GameSettingInput : GameSetting, ISelectHandler
    {
        InputField _text;

        protected override void OnStart()
        {
            base.OnStart();

            _text = GetComponent<InputField>();

            _text.onEndEdit.AddListener(EndEdit);
#if UNITY_5_3
            _text.onValueChanged.AddListener(ValueChanged);
#else
            _text.onValueChange.AddListener(ValueChanged);
#endif

            SetValue(_settingsData.GetConfig(settingName, _controllerType));
            _text.text = settingValue;
        }
        
        void OnValueChange()
        {
            SetValue(_text.text);
        }

        protected virtual void EndEdit(string value)
        {
            OnValueChange();
            _settingsData.SetConfig(settingName, settingValue, _controllerType);
        }
        protected virtual void ValueChanged(string value)
        {
            OnValueChange();
        }
        public virtual void OnSelect(BaseEventData eventData)
        {
            _text.text = _settingsData.GetConfig(settingName, _controllerType);
        }
    }
}