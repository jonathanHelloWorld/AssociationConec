using UnityEngine;
using System.Collections;
using System;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/DataModel/ Memory")]
    public class MemoryData : DataModel
    {
        void Awake()
        {
            Type = ModelTypes.MemoryData;
        }

        [Serializable]
        public struct CardData
        {
            public Sprite[] sprites;
        }

        public CardData[] Cards;
        public Sprite cardBack;
    }
}