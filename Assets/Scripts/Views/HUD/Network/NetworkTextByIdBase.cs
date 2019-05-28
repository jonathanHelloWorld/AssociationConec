using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkTextByIdBase : TextView
    {
        public int Index;

        [SerializeField]
        protected string myUid;

        public bool OrderByRanking;
        public bool ShowPrepared = false;

        protected NetworkController _network;
        protected RankingController _ranking;
        protected IdsController _ids;
        protected ScoreController _score;

        protected override void OnStart()
        {
            base.OnStart();

            _network = _controller as NetworkController;

            _network.OnAddClient += UpdateData;
            _network.OnRemoveClient += UpdateData;
            _network.OnClientStatsChange += UpdateData;

            _ids = _bootstrap.GetController(ControllerTypes.Id) as IdsController;

#if HAS_SERVER
            _ids.OnSessionUpdate += UpdateData;
            _ids.OnIdsReceiveEnded += UpdateData;
#endif

            _ranking = _bootstrap.GetController(ControllerTypes.Ranking) as RankingController;
            _score = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            
            _score.OnValueUpdate += UpdateData;

            ResetText();
        }
        
        void ResetText()
        {
            _tx.text = "";
        }

        protected override void ResetView()
        {
            base.ResetView();
            ResetText();
        }

        protected virtual void UpdateData()
        {
#if HAS_SERVER
            if (OrderByRanking)
            {
                var sscc = _ranking.GetSessionConnectionsOrdered(NetworkInstanceType.ActiveClient, ShowPrepared);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;
            }
            else
            {
                var sscc = _network.GetSessionConnections(NetworkInstanceType.ActiveClient, ShowPrepared);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;
            }
#endif
        }
        protected virtual void UpdateData(NetworkClientObject client)
        {
#if HAS_SERVER
            if (OrderByRanking)
            {
                var sscc = _ranking.GetSessionConnectionsOrdered(NetworkInstanceType.ActiveClient, ShowPrepared);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;

                if (client.uid != myUid) return;
            }
            else
            {
                var sscc = _network.GetSessionConnections(NetworkInstanceType.ActiveClient, ShowPrepared);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;

                if (client.uid != myUid) return;
            }
#endif
        }
    }
}