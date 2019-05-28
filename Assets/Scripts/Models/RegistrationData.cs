using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using System.Linq;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Register")]
    public class RegistrationData : DataModel
    {
        /// <summary>
        /// eventos de atualizacao do cadastro seja para novo ou modificado
        /// </summary>
        public event GenericController.SimpleEvent OnNewRegstry, OnRegstryUpdate;

#if HAS_SERVER
        public event GenericController.SimpleEvent OnRegistryReceiveEnded;
        private bool isReceiving;
        public bool SedToServerWhenListener;
#endif

        /// <summary>
        /// Lista com todos os dados de cadastro
        /// </summary>
        [SerializeField]
        protected List<RegistryData> Register;

        /// <summary>
        /// Usuario atual usado para referencia na lista de cadastro
        /// </summary>
        [Space]
        public int ActualUser;

        #region Initialization
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Register;
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void ResetData()
        {
            Register = new List<RegistryData>();
            ActualUser = -1;
        }
        protected override void GetReferences()
        {
            _idsData.OnNewId += NewRegistry;

            ActualUser = Register.Count-1;

#if HAS_SERVER
            if (_isServer) return;

            if (GetDataFromServerOnConnect)
                _clientController.OnConnect += NetworkRequestRegistryFromServer;

            if (!SedToServerWhenListener && _clientController.GetInstanceType() == NetworkInstanceType.ListeningClient) return;

            _bootstrap.GameControllerStarted += NetworkSendRegistryToServer;
            OnNewRegstry += NetworkSendRegistryToServer;
            OnRegstryUpdate += NetworkSendRegistryToServer;
#endif
        }
        public override void CallReset()
        {
            base.CallReset();
        }
        #endregion

        #region GetSetters
        //Getters
        public int GetFilteredRegistryCount()
        {
            return Register.FindAll(x => x.registry.Count > 5).Count;
        }
        public int GetRegistryCount()
        {
            return Register.Count;
        }
        public RegistryData GetRegistry(string uid)
        {
            return Register.Exists(x => x.registry["UniqueId"] == uid) ? Register.Find(x => x.registry["UniqueId"] == uid) : null;
        }
        
        public RegistryData GetActualRegistry()
        {
            if (ActualUser < 0)
                return null;

            return Register[ActualUser];
        }
        public string GetActualRegistry(string fieldName)
        {
            if (Register.ElementAtOrDefault(ActualUser) == null) return null;

            if (ActualUser >= 0 && Register[ActualUser].registry.ContainsKey(fieldName))
                return Register[ActualUser].registry[fieldName];
            
            return null;
        }
        public string GetRegistryValue(int index, string fieldName)
        {
            if (index >= 0 && index < Register.Count && Register[index].registry.ContainsKey(fieldName))
                return Register[index].registry[fieldName];

            return null;
        }
        public string GetRegistryValue(string uid, string fieldName)
        {
            int i;
            if (Register.Exists(x => x.registry["UniqueId"] == uid))
                i = Register.FindIndex(x => x.registry["UniqueId"] == uid);
            else
                return null;

            if (Register[i].registry.ContainsKey(fieldName))
                return Register[i].registry[fieldName];

            return null;
        }
        public bool TryGetRegistryValue(string fieldName, out string value)
        {
            value = null;

            if (Register.Exists(x => x.registry.ContainsKey(fieldName)))
            {
                value = Register.Find(x => x.registry.ContainsKey(fieldName)).registry[fieldName];
                return true;
            }

            return false;
        }
        public bool TryGetRegistryValue(string fieldName, out string value, bool isActual)
        {
            value = null;

            if (isActual)
            {
                var index = ActualUser;

                if (index >= 0 && index < Register.Count && Register[index].registry.ContainsKey(fieldName))
                {
                    value = Register[index].registry[fieldName];
                    return true;
                }
            }

            if (Register.Exists(x => x.registry.ContainsKey(fieldName)))
            {
                value = Register.Find(x => x.registry.ContainsKey(fieldName)).registry[fieldName];
                return true;
            }

            return false;
        }
        public bool TryGetRegistryFromValue(string fieldName, string fieldValue, out string value)
        {
            value = "";

            if (Register.Exists(x => x.registry.ContainsKey(fieldName) && x.registry[fieldName] == fieldValue))
            {
                value = Register.Find(x => x.registry.ContainsKey(fieldName) && x.registry[fieldName] == fieldValue).registry[fieldName];
                return true;
            }

            return false;
        }
        public bool TryGetRegistryValue(int index, string fieldName, out string value)
        {
            value = "";

            if (index >= 0 && index < Register.Count && Register[index].registry.ContainsKey(fieldName))
            {
                value = Register[index].registry[fieldName];
                return true;
            }

            return false;
        }
        public bool TryGetRegistryValue(string uid, string fieldName, out string value)
        {
            value = "";
            int i;
            if (Register.Exists(x => x.registry["UniqueId"] == uid))
                i = Register.FindIndex(x => x.registry["UniqueId"] == uid);
            else
                return false;

            if (Register[i].registry.ContainsKey(fieldName))
            {
                value = Register[i].registry[fieldName];
                return true;
            }

            return false;
        }

        //Setters
        void NewRegistry(Id uid)
        {
            Register.Add(new RegistryData(uid.uid));
            
            ActualUser = Register.Count-1;
            AddRegistryDefaultInfo();

#if HAS_SERVER
            if (isReceiving) return;
#endif
            if (OnNewRegstry != null) OnNewRegstry();
            base.AddNewValue();
        }
        public void NewRegistry(RegistryData value)
        {
            if (Register.Exists(x => x.registry["UniqueId"] == value.registry["UniqueId"]))
            {
                var i = Register.FindIndex(x => x.registry["UniqueId"] == value.registry["UniqueId"]);
                Register[i] = value;
            }
            else
            {
                Register.Add(value);
#if HAS_SERVER
                //if (!isReceiving)
                    //ActualUser = Register.Count - 1;
#endif
            }

            base.AddNewValue();

#if HAS_SERVER
            if (isReceiving) return;
#endif
            if (OnNewRegstry != null) OnNewRegstry();
        }
        public void NewRegistryValue(string key, string value, bool updateTime)
        {
            if (Register[ActualUser].registry.ContainsKey(key))
            {
                Register[ActualUser].registry[key] = value;
            }
            else
            {
                Register[ActualUser].registry.Add(key, value);
            }

            if (updateTime)
            {
                Register[ActualUser].registry["Date"] = System.DateTime.Now.ToShortDateString();
                Register[ActualUser].registry["Time"] = System.DateTime.Now.ToShortTimeString();
            }
            
            if (OnRegstryUpdate != null) OnRegstryUpdate();
        }

        void AddRegistryDefaultInfo()
        {
            Register[ActualUser].registry.Add("Date", System.DateTime.Now.ToShortDateString());
            Register[ActualUser].registry.Add("Time", System.DateTime.Now.ToShortTimeString());
            Register[ActualUser].registry.Add("Project", Application.productName);
            Register[ActualUser].registry.Add("ProjectNumber", _idsData.projectNumber);
        }
#endregion

#region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Register, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            Register = JsonConvert.DeserializeObject<List<RegistryData>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            throw new NotImplementedException();
        }

        public override string SerializeDataBaseToCSV()
        {
            var csv = "";
            var keys = Register[0].registry.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                csv += keys[i] + ";";
            }
            csv += Environment.NewLine;
            for (int i = 0; i < Register.Count; i++)
            {
                keys = Register[i].registry.Keys.ToList();
                for (int j = 0; j < keys.Count; j++)
                {
                    csv += Register[i].registry[keys[j]] + ";";
                }
                csv += Environment.NewLine;
            }
            return csv;
        }

        #endregion

#region Networking specific methods
#if HAS_SERVER
#region Server methods
        public void NetworkUpdateRegister(string json)
        {
            if (!_isServer) return;

            NewRegistry(JsonConvert.DeserializeObject<RegistryData>(json));
            _serverController.SendMessageToType("NetworkSetRegistry", json, NetworkInstanceType.ListeningClient);
        }

        public void NetworkRequestRegistry(string json)
        {
            if (!_isServer) return;

            _serverController.SendMessageToAll("NetworkStartRceiveRegistry", "");

            for (int i = 0, n = Register.Count; i < n; i++)
            {
                _serverController.SendMessageToAll("NetworkSetRegistry", JsonConvert.SerializeObject(Register[i]));
            }

            _serverController.SendMessageToAll("NetworkSetRegistryEnd", "");

            DebugLog("RequestRegistry - All Data Send");
        }
#endregion

#region Client methods
        public void NetworkSendRegistryToServer()
        {
            if (_isServer || isReceiving) return;

            _clientController.SendMessageToServer("NetworkUpdateRegister", JsonConvert.SerializeObject(GetActualRegistry()));
        }
        public void NetworkRequestRegistryFromServer()
        {   
            if (_isServer) return;
            _clientController.SendMessageToServer("NetworkRequestRegistry", "");
        }

#region Receive all data from server process
        public void NetworkStartRceiveRegistry(string json)
        {
            if (_isServer) return;

            isReceiving = true;
            Register = new List<RegistryData>();
        }
        public void NetworkSetRegistry(string json)
        {
            if (_isServer) return;

            isReceiving = true;
            NewRegistry(JsonConvert.DeserializeObject<RegistryData>(json));

            isReceiving = false;
        }
        public void NetworkSetRegistryEnd(string json)
        {
            if (_isServer) return;

            ActualUser = Register.Count - 1;
            NewRegistry(_idsData.GetActual());

            Save();

            isReceiving = false;
            if (OnRegistryReceiveEnded != null) OnRegistryReceiveEnded();
        }
#endregion

#endregion
#endif
#endregion
    }

    [System.Serializable]
    public class RegistryData
    {
        public Dictionary<string, string> registry; 

        public RegistryData()
        {
            registry = new Dictionary<string, string>();
        }
        public RegistryData(string uid)
        {
            registry = new Dictionary<string, string>();
            registry.Add("UniqueId", uid);
        }
    }
}