using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using InterativaSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using InterativaSystem.Views.Others.Settings;

namespace InterativaSystem.Views.HUD.Page
{
    public class PageGameSettingsApplyAuto : DoOnPageAuto
    {
        public List<GameSettings2> Settings;

        protected override void DoSomething()
        {
            ApplyConfig();
        }

        public void ApplyConfig()
        {
            for (int i = 0; i < Settings.Count; i++)
            {
                GameSettings2 gameSetting = Settings[i];

                var controller = _bootstrap.GetController(gameSetting.controller);

                if (controller != null)
                {
                    var props = controller.GetType().GetFields().ToList();

                    for (int j = 0; j < props.Count; j++)
                    {
                        int index = gameSetting.settings.FindIndex(x => x.settingName.Equals(props[j].Name));
                        if (index != -1)
                        {
                            props[j].SetValue(controller, Convert.ChangeType(gameSetting.settings[index].settingValue, props[j].GetValue(controller).GetType()), BindingFlags.Instance | BindingFlags.Public, null, null);
                        }
                    }
                }
                else
                    UnityEngine.Debug.LogError("No Controller found: " + gameSetting.controller.ToString());
            }
        }
    }

    [Serializable]
    public class GameSettings2
    {
        public ControllerTypes controller;
        public int id;

        public List<GameSetting2> settings;

        public GameSettings2()
        {
            settings = new List<GameSetting2>();
        }
    }

    [Serializable]
    public class GameSetting2
    {
        public string settingName;
        public string settingValue;
    }
}