using System.Collections.Generic;
using Newtonsoft.Json;
using InterativaSystem.Controllers;
using InterativaSystem.Views.Others.Settings;
using System.Linq;
using System.Reflection;
using System;
using System.ComponentModel;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Game Settings")]
    public class GameSettingsData : DataModel
    {
        public List<GameSettings> Settings;

        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.GameSettings;
        }

        public void ApplyConfig()
        {
            for(int i = 0; i < Settings.Count; i++)
            {
                GameSettings gameSetting = Settings[i];

                var controller = _bootstrap.GetController(gameSetting.controller);
                var props = controller.GetType().GetFields().ToList();

                for (int j = 0; j < props.Count; j++)
                {
                    if (gameSetting.settings.ContainsKey(props[j].Name))
                    {
                        Debug.Log(gameSetting.settings[props[j].Name].settingValue + " - " + props[j].GetValue(controller).GetType());
                        props[j].SetValue(controller, Convert.ChangeType(gameSetting.settings[props[j].Name].settingValue, props[j].GetValue(controller).GetType()), BindingFlags.Instance | BindingFlags.Public, null, null);
                    }
                }
            }
        }

        public void SetConfig(string key, string value, ControllerTypes controllerType)
        {
            var controller = _bootstrap.GetController(controllerType);
            var props = controller.GetType().GetFields().ToList();

            if (props.Exists(x => x.Name == key))
            {
                var prop = props.Find(x => x.Name == key);
                prop.SetValue(controller, Convert.ChangeType(value, prop.GetValue(controller).GetType()), BindingFlags.Instance | BindingFlags.Public, null, null);
            }
            
        }
        public string GetConfig(string key, ControllerTypes controllerType)
        {
            var controller = _bootstrap.GetController(controllerType);
            var props = controller.GetType().GetFields().ToList();

            return props.Find(x => x.Name == key).GetValue(controller).ToString();
        }

        //This methods will serialize an deserialize from a json data
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Settings);
        }
        public override void DeserializeDataBase(string json)
        {
            Settings = JsonConvert.DeserializeObject<List<GameSettings>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<GameSettings>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (Settings.Exists(x => x.id == data[i].id))
                {

                }
                else
                {
                    Settings.Add(data[i]);
                }
            }
        }
    }

    [Serializable]
    public class GameSettings
    {
        public ControllerTypes controller;
        public int id;

        public Dictionary<string, GameSetting> settings;

        public GameSettings()
        {
            settings = new Dictionary<string, GameSetting>();
        }
    }
}