using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Ranking Controller")]
    public class RankingController : GenericController
    {
        private ScoreData _scoreData;
        private IdsData _idsData;

        public event ScoreData.ListScoreEvent OnScoreUpdate;
        
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Ranking;
        }

        protected override void OnStart()
        {
            _scoreData = _bootstrap.GetModel(ModelTypes.Score) as ScoreData;

            _scoreData.OnDataUpdated += Sort;

            _idsData = _bootstrap.GetModel(ModelTypes.Ids) as IdsData;

            //_bootstrap.GameControllerEnded += Sort;
        }

        #region GetSetters
        public bool TryGetPosition(string id, out int position)
        {
            var plyrsPos = _scoreData.GetSessionValues();

            plyrsPos = plyrsPos.OrderBy(x => x.time).ToList();
            plyrsPos = plyrsPos.OrderByDescending(x => x.value).ToList();

            position = -1;

            if (plyrsPos.Exists(x => x.uid == id))
            {
                if (plyrsPos.Any(x => x.value != plyrsPos[0].value))
                    position = plyrsPos.FindIndex(x => x.uid == id);
                else
                    position = 0;

                return true;
            }
            return false;
        }
        public bool TryGetPosition(out int position)
        {
            var plyrsPos = _scoreData.GetSessionValues();
            var id = _idsData.GetActual().uid;

            plyrsPos = plyrsPos.OrderBy(x => x.time).ToList();
            plyrsPos = plyrsPos.OrderByDescending(x => x.value).ToList();

            position = -1;

            if (plyrsPos.Exists(x => x.uid == id))
            {
                position = plyrsPos.FindIndex(x => x.uid == id);
                return true;
            }
            return false;
        }
#if HAS_SERVER
        public List<ScoreValue> GetSessionConnectionsOrdered(NetworkInstanceType type)
        {
            var plyrsPos = _scoreData.GetSessionValues(type);

            plyrsPos = plyrsPos.OrderBy(x => x.time).ToList();
            plyrsPos = plyrsPos.OrderByDescending(x => x.value).ToList();

            return plyrsPos;
        }

        public List<ScoreValue> GetSessionConnectionsOrdered(NetworkInstanceType type, bool show)
        {
            var plyrsPos = _scoreData.GetSessionValues(type, show);

            plyrsPos = plyrsPos.OrderBy(x => x.time).ToList();
            plyrsPos = plyrsPos.OrderByDescending(x => x.value).ToList();

            return plyrsPos;
        }
#endif
        #endregion

        public void Sort()
        {
            var plyrsPos = _scoreData.GetSessionValues();

            plyrsPos = plyrsPos.OrderBy(x => x.time).ToList();
            plyrsPos = plyrsPos.OrderByDescending(x => x.value).ToList();
            //plyrsPos.Sort((a,b) =>b.value.CompareTo(a.value));

            if (OnScoreUpdate != null) OnScoreUpdate(plyrsPos);
        }
    }
}