using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfo = InterativaSystem.Models.CharacterInfo;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkCharacterById : GenericView
    {
        public int Index;

        [SerializeField]
        protected string myUid;

        protected NetworkController _network;
        protected RankingController _ranking;
        protected IdsController _ids;
        protected ScoreController _score;


        public bool OrderByRanking;

        public GameObject[] Characters;

        private CharacterSelectionController _character;

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

            //_image = GetComponent<Image>();

            _character = _bootstrap.GetController(ControllerTypes.Characters) as CharacterSelectionController;
            _character.OnCharacterUpdated += UpdateData;
        }

        protected override void ResetView()
        {
            base.ResetView();

            for (int i = 0; i < Characters.Length; i++)
                Characters[i].SetActive(false);
        }
        
        protected void UpdateData()
        {
#if HAS_SERVER
            for(int i = 0; i < Characters.Length; i++)
            {
                Characters[i].SetActive(false);
            }

            if (OrderByRanking)
            {
                var sscc = _ranking.GetSessionConnectionsOrdered(NetworkInstanceType.ActiveClient);
                
                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;

                if (!_network.GetConnection(myUid).isGamePrepared || string.IsNullOrEmpty(myUid)) return;

                CharacterInfo ch = null;

                if (_character.TryGetCharacter(myUid, out ch))
                {
                    if (Characters.Length > ch.id)
                        Characters[ch.id].SetActive(true);
                        //_image.sprite = Characters[ch.id];
                }
            }
            else
            {
                var sscc = _network.GetSessionConnections(NetworkInstanceType.ActiveClient);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;

                if (!_network.GetConnection(myUid).isGamePrepared || string.IsNullOrEmpty(myUid)) return;

                CharacterInfo ch = null;

                if (_character.TryGetCharacter(myUid, out ch))
                {
                    if (Characters.Length > ch.id)
                        Characters[ch.id].SetActive(true);
                }
            }
#endif
        }

        protected void UpdateData(NetworkClientObject client)
        {
#if HAS_SERVER
            if (OrderByRanking)
            {
                var sscc = _ranking.GetSessionConnectionsOrdered(NetworkInstanceType.ActiveClient);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;

                if (client.uid != myUid) return;

                if (string.IsNullOrEmpty(myUid)) return;

                CharacterInfo ch = null;

                if (_character.TryGetCharacter(myUid, out ch))
                {
                    if (Characters.Length > ch.id)
                        Characters[ch.id].SetActive(true);
                }
            }
            else
            {
                var sscc = _network.GetSessionConnections(NetworkInstanceType.ActiveClient);

                if (sscc.Count <= Index)
                {
                    myUid = null;
                    return;
                }

                myUid = sscc[Index].uid;

                if (string.IsNullOrEmpty(myUid)) return;

                CharacterInfo ch = null;

                if (_character.TryGetCharacter(myUid, out ch))
                {
                    if (Characters.Length > ch.id)
                        Characters[ch.id].SetActive(true);
                }
            }
#endif
        }
    }
}