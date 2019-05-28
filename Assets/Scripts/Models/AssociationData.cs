using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Models
{
    public class AssociationData : DataModel
    {
        //Todo: completar o usao desse model plara usar um instanciador dinamico
        [SerializeField]
        protected List<AssociationRound> Rounds;
        protected int _actualRound;

        //Todo: Usar IContainer
        public Transform Root;

        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Association;
        }

        public override void CallReset()
        {
            base.CallReset();
            Rounds.Shuffle();
        }

        #region Setters
        public AssociationRound GetActualRound()
        {
            return Rounds[_actualRound];
        }
        public List<AssociationPiece> GetPieces(int rooundId)
        {
            return Rounds[rooundId].pieces;
        }
        public AssociationPiece GetPiece(int rooundId, int pieceId)
        {
            return Rounds[rooundId].pieces[pieceId];
        }
        public int GetRound()
        {
            return _actualRound;
        }
        public bool NewRound()
        {
            _actualRound++;
            if (_actualRound >= Rounds.Count)
            {
                _actualRound = 0;
                return false;
            }
            return true;
        }
        #endregion

        #region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(Rounds, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            Rounds = JsonConvert.DeserializeObject<List<AssociationRound>>(json);
            Rounds.Shuffle();
        }

        public override void UpdateDataBase(string folderData)
        {
            throw new NotImplementedException();
        }

        #endregion

        public string GetRoundTittle()
        {
            return Rounds[_actualRound].title;
        }
        public string GetRoundTittle(bool isB)
        {
            return isB ? Rounds[_actualRound].titleB : Rounds[_actualRound].title;
        }
    }

    [System.Serializable]
    public class AssociationRound
    {
        public int id;
        public string title, titleB;

        public List<AssociationPiece> pieces;
    }

    [System.Serializable]
    public class AssociationPiece
    {
        public string text;
        public string sprite;
        public int id;
        public List<int> ids;
        public Vector3 pos;
        public bool useSize;
        public Vector3 size;
        public bool isStatic;
    }
}