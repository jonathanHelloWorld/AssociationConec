using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    public enum PrefabCategory
    {
        Null = 0,
        Coin = 11,
        Obstacle = 12,
        Environment = 15,
        Character = 1,
        AssociationObjectDynamic,
        AssociationObjectStatic,
        UILineText = 20,
        UIStructure = 25,
        UICheckInLineText,
        UITurningLine,
        MemoryCardPrefab,
        CandyPiece,
        CandyGel,
        CandyVertical,
        CandyHorizontal,
        CandyDestroyEffect
    }

    [AddComponentMenu("ModularSystem/DataModel/ Resources")]
    public class ResourcesDataBase : DataModel
    {
        //TODO Yuri: resources deve ter um controller para controlar instancias
        private RegistrationData _data;
        
        [Space]
        public List<Prefabinfo> Prefabs;

        #region MonoBehaviour Methods
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Resources;
        }
        protected override void OnStart()
        {
            if (OverrideDB)
                NormalizeProbability();

            if (!OverrideDB && !_IOController.TryLoad(this))
            {
                _IOController.Save(this);
            }
            else if (OverrideDB)
            {
                _IOController.Save(this);
            }
        }
        #endregion

        //This methods will serialize and deserialize from a json data
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Prefabs);
        }
        public override void DeserializeDataBase(string json)
        {
            Prefabs = JsonConvert.DeserializeObject<List<Prefabinfo>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<Prefabinfo>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (Prefabs.Exists(x => x.id == data[i].id && x.category == data[i].category))
                {
                    Prefabs.Find(x => x.id == data[i].id && x.category == data[i].category).name = data[i].name;
                    Prefabs.Find(x => x.id == data[i].id && x.category == data[i].category).category = data[i].category;
                }
                else
                {
                    Prefabs.Add(data[i]);
                }
            }
        }

        void NormalizeProbability()
        {
            //Get only one data by PrefabCategory
            var distinctValues =
                from cust in Prefabs
                group cust by cust.category
                into gcust
                select gcust.First();

            var categoriesList = distinctValues.ToList();

            for (int i = 0, n = categoriesList.Count(); i < n; i++)
            {
                var prefabsFrom = Prefabs.FindAll(x => x.category == categoriesList[i].category);
                //Clamp
                for (int j = 0, m = prefabsFrom.Count; j < m; j++)
                {
                    prefabsFrom[j].probability = prefabsFrom[j].probability > 1 ? 1 : prefabsFrom[j].probability < 0 ? 0 : prefabsFrom[j].probability;
                }
                var sum = prefabsFrom.Sum(x => x.probability);
                for (int j = 0, m = prefabsFrom.Count; j < m; j++)
                {
                    prefabsFrom[j].probability = prefabsFrom[j].probability/sum;
                }
            }
        }

        public Prefabinfo GetPrefabByProbability(float range, PrefabCategory category)
        {
            //Get only one data by PrefabCategory
            var distinctValues = Prefabs.FindAll(x => x.category == category);

            range /= distinctValues.Count();
            var probs =  Prefabs.FindAll(x => x.category == category && x.probability >= range);
            var rnd = new System.Random();

            return probs[rnd.Next(0, probs.Count)];
        }
    }

    [Serializable]
    public class Prefabinfo
    {
        public string name;
        public int id;
        public PrefabCategory category;
        [Range(0, 1)]
        public float probability = 1;
    }
}