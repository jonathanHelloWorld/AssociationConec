using UnityEngine;
using System.Collections;
using InterativaSystem.Models;
using System.Collections.Generic;

namespace InterativaSystem.Views.Others.Settings
{
    public class GameSetting : GenericView
    {
        public string settingName;
        public string settingValue;

        protected GameSettingsData _settingsData;

        protected override void OnStart()
        {
            base.OnStart();

            _settingsData = _bootstrap.GetModel(ModelTypes.GameSettings) as GameSettingsData;

            if (_settingsData != null)
            {
                GameSettings gameSettings = _settingsData.Settings.Find(x => x.controller == _controllerType);

                if (gameSettings != null)
                {
                    if (!gameSettings.settings.ContainsKey(settingName))
                        gameSettings.settings.Add(settingName, this);
                }
                else
                    Debug.LogError("Controller Settings not set.");
            }
        }

        public void SetValue(float value)
        {
            SetValue(value.ToString());
        }

        public void SetValue(int value)
        {
            SetValue(value.ToString());
        }

        public void SetValue(string value)
        {
            settingValue = value;
        }
    }
}