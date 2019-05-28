using System.Collections.Generic;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Groups")]
    public class GroupsInfo : DataModel
    {
        public event GenericController.SimpleEvent GroupUpdated;

        public List<Group> Groups;

        private void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Group;

            if (!OverrideDB)
                Groups = new List<Group>();
        }
        protected override void OnStart()
        {
            if (!OverrideDB && !_IOController.TryLoad(this))
            {
                _IOController.Save(this);
            }
            else if (OverrideDB)
            {
                _IOController.Save(this);
            }

#if HAS_SERVER
            //TEMPORARY SOLUTION REMOVE AS SOON AS POSSIBLE
            if (!_isServer || _isServer)
            {
                for (int i = 0; i < Groups.Count; i++)
                {
                    Groups[i].AreaPoints = 0;
                }
            }
#endif
        }

        //This methods will serialize an deserialize from a json data
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Groups, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            Groups = JsonConvert.DeserializeObject<List<Group>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<Group>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (Groups.Exists(x => x.Id == data[i].Id))
                {
                    Groups.Find(x => x.Id == data[i].Id).AreaPoints = data[i].AreaPoints;
                    Groups.Find(x => x.Id == data[i].Id).Color = data[i].Color;
                    Groups.Find(x => x.Id == data[i].Id).Points = data[i].Points;
                    Groups.Find(x => x.Id == data[i].Id).Name = data[i].Name;
                    Groups.Find(x => x.Id == data[i].Id).TimeSum = data[i].TimeSum;
                }
                else
                {
                    Groups.Add(data[i]);
                }
            }
        }


#if HAS_SERVER
        public void NetworkSendGroups()
        {
            if (_isServer)
            {
                DebugLog("Sending Groups Data");
                for (int i = 0, n = Groups.Count; i < n; i++)
                {
                    _serverController.SendMessageToAll("NetworkReceiveGroup", JsonConvert.SerializeObject(Groups[i]));
                }

                _serverController.SendMessageToAll("NetworkReceiveGroupsEnded", "");

                for (int i = 0, n = Groups.Count; i < n; i++)
                {
                    Groups[i].AreaPoints = Groups[i].Points;
                }
            }
        }
        public void NetworkReceiveGroup(string json)
        {
            var group = JsonConvert.DeserializeObject<Group>(json);

            if (Groups.Exists(x => x.Id == group.Id))
            {
                Groups.Find(x => x.Id == group.Id).Points = group.Points;
                Groups.Find(x => x.Id == group.Id).AreaPoints = group.AreaPoints;
            }
            else
            {
                Groups.Add(group);
            }
        }
        public void NetworkReceiveGroupsEnded(string json)
        {
            if (GroupUpdated != null) GroupUpdated();
        }
#endif
    }

#if HAS_SQLITE3
    public class Group
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public float ColorR { get; set; }
        public float ColorG { get; set; }
        public float ColorB { get; set; }
        public float ColorA { get; set; }

        public int Points { get; set; }
        public int AreaPoints { get; set; }
        public float TimeSum { get; set; }

        [Ignore]
        public Vector4 Color { get; set; }
    }
#else
    [System.Serializable]
    public class Group
    {
        public int Id;
        public string Name;
        public Vector4 Color;

        public int Points;
        public int AreaPoints;
        public float TimeSum;
    }
#endif

}