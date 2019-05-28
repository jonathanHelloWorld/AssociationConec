using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.Others.Settings
{
    [RequireComponent(typeof(Text))]
    public class GameSettingText : GameSetting
    {
        Text _tx;

        public bool UpdateValue = true;

        protected override void OnStart()
        {
            base.OnStart();

            _tx = GetComponent<Text>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!UpdateValue) return;

            var vl = _settingsData.GetConfig(settingName, _controllerType);

            if (!string.IsNullOrEmpty(vl))
                _tx.text = vl;
        }
    }
}