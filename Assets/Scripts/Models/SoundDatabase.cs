using System.Collections.Generic;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    public enum SoundCategory
    {
        SFX = 0,
        BGM = 1
    }
    [AddComponentMenu("ModularSystem/DataModel/ Sounds Database")]
    public class SoundDatabase : DataModel
    {
        [Space]
        public List<SoundPropierty> SoundPropierties;

        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Sound;
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
        }


        //This methods will serialize and deserialize from a json data
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(SoundPropierties);
        }
        public override void DeserializeDataBase(string json)
        {
            SoundPropierties = JsonConvert.DeserializeObject<List<SoundPropierty>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<SoundPropierty>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (SoundPropierties.Exists(x => x.id == data[i].id && x.category == data[i].category))
                {
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).name = data[i].name;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).ReverbZoneMix = data[i].ReverbZoneMix;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).category = data[i].category;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).clip = data[i].clip;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).loop = data[i].loop;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).pitch = data[i].pitch;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).spatialBlend = data[i].spatialBlend;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).stereoPan = data[i].stereoPan;
                    SoundPropierties.Find(x => x.id == data[i].id && x.category == data[i].category).volume = data[i].volume;
                }
                else
                {
                    SoundPropierties.Add(data[i]);
                }
            }
        }
    }

    [System.Serializable]
    public class SoundPropierty
    {
        public string name;
        public int id;
        public SoundCategory category;

        public AudioClip clip;

        public bool loop;

        [Range(0,1)]
        public float volume = 1;
        [Range(-3, 3)]
        public float pitch = 1;
        [Range(-1, 1)]
        public float spatialBlend;
        [Range(-1, 1)]
        public float stereoPan;
        [Range(-1, 1)]
        public float ReverbZoneMix;
    }
}