using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    public enum ImageCategory
    {
        Null = 0,
        CandyImage,
        CandyHorizontal,
        CandyVertical,
        CandySpecial,
        MemoryCard,
        MemoryBack
    }

    [AddComponentMenu("ModularSystem/DataModel/ Images")]
    public class ImagesDataBase : DataModel
    {
        //Todo Diego: deve ser vinculado ao IO caso as imagens tb sejam dinamicas
        [Space]
        [SerializeField]
        protected List<ImageInfo> Images;

        #region MonoBehaviour Methods
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Images;
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
            return JsonConvert.SerializeObject(Images);
        }
        public override void DeserializeDataBase(string json)
        {
            Images = JsonConvert.DeserializeObject<List<ImageInfo>>(json);
        }
        public override void UpdateDataBase(string folderData)
        {
            var data = JsonConvert.DeserializeObject<List<ImageInfo>>(folderData);

            for (int i = 0, n = data.Count; i < n; i++)
            {
                if (Images.Exists(x => x.id == data[i].id && x.category == data[i].category))
                {
                    Images.Find(x => x.id == data[i].id && x.category == data[i].category).name = data[i].name;
                    Images.Find(x => x.id == data[i].id && x.category == data[i].category).category = data[i].category;
                }
                else
                {
                    Images.Add(data[i]);
                }
            }
        }

        void NormalizeProbability()
        {
            //Get only one data by PrefabCategory
            var distinctValues =
                from cust in Images
                group cust by cust.category
                into gcust
                select gcust.First();

            var categoriesList = distinctValues.ToList();

            for (int i = 0, n = categoriesList.Count(); i < n; i++)
            {
                var imagesFrom = Images.FindAll(x => x.category == categoriesList[i].category);
                //Clamp
                for (int j = 0, m = imagesFrom.Count; j < m; j++)
                {
                    imagesFrom[j].probability = imagesFrom[j].probability > 1 ? 1 : imagesFrom[j].probability < 0 ? 0 : imagesFrom[j].probability;
                }
                var sum = imagesFrom.Sum(x => x.probability);
                for (int j = 0, m = imagesFrom.Count; j < m; j++)
                {
                    imagesFrom[j].probability = imagesFrom[j].probability / sum;
                }
            }
        }

        public Sprite GetImageByProbability(float range, ImageCategory category)
        {
            //Get only one data by PrefabCategory
            var distinctValues = Images.FindAll(x => x.category == category);

            range /= distinctValues.Count();
            var probs = Images.FindAll(x => x.category == category && x.probability >= range);
            var rnd = new System.Random();

            return Resources.Load<Sprite>(probs[rnd.Next(0, probs.Count)].name);
        }

        public Sprite GetImageByName(string name)
        {
            return Resources.Load<Sprite>(Images.Find(x => x.name == name).name);
        }

        public Sprite GetImageByID(int id)
        {
            return Resources.Load<Sprite>(Images.Find(x => x.id == id).name);
        }

        public Sprite GetImageByCategory(ImageCategory category)
        {
            return Resources.Load<Sprite>(Images.Find(x => x.category == category).name);
        }

        public List<Sprite> GetImagesByCategory(ImageCategory category)
        {
            List<ImageInfo> images = Images.FindAll(x => x.category == category);
            List<Sprite> sprites = new List<Sprite>();

            for(int i = 0; i < images.Count; i++)
            {
                sprites.Add(Resources.Load<Sprite>(images[i].name));
            }

            return sprites;
        }

        public List<List<Sprite>> GetImagesByCategory(ImageCategory category, bool groupByID)
        {
            if (!groupByID) return null;

            List<ImageInfo> images = Images.FindAll(x => x.category == category);
            List<List<Sprite>> sprites = new List<List<Sprite>>();

            int index = images.Max(x => x.id);

            for(int i = 0; i < index; i++)
            {
                sprites.Add(new List<Sprite>());
            }

            for (int i = 0; i < images.Count; i++)
            {
                sprites[images[i].id - 1].Add(Resources.Load<Sprite>(images[i].name));
            }

            return sprites;
        }
    }

    [Serializable]
    public class ImageInfo
    {
        public string name;
        public int id;
        public ImageCategory category;
        [Range(0, 1)]
        public float probability = 1;
    }
}