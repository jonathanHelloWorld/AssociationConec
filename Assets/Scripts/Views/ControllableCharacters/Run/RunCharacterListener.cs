using System;
using DG.Tweening;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters
{
    public class RunCharacterListener : RunCharacter
    {
        private RankingController _rankingController;

        public float Offset = 2;
        public string Id;

        public Vector3 EndRotation;
        public float RotationDuration;

        [HideInInspector]
        public int trackId;

        private int _counter;

        private float _offsetFromTrackAnimated;

        private NetworkInstanceType networkType;

        protected override void OnStart()
        {
            base.OnStart();

            _rankingController = _bootstrap.GetController(ControllerTypes.Ranking) as RankingController;
            
            actualTrack = trackId;

            transform.position = _tracksInfo.Tracks[trackId].GetPointOnTrack(0);

            if (_bootstrap.GetController(ControllerTypes.NetworkClient) != null) 
                networkType = ((NetworkClientController) _bootstrap.GetController(ControllerTypes.NetworkClient)).GetInstanceType();
        }

        protected override void OnUpdate()
        {
            //TODO Diego: Refazer logica do RunListener
            //TODO Diego: remover do update e criar vinculo de id com o servidor
#if HAS_SERVER
            int index;

            var pos = _rankingController.GetSessionConnectionsOrdered(NetworkInstanceType.ActiveClient);

            if (_rankingController.TryGetPosition(Id, out index))//.PlayersPositions.Exists(x => x.id == Id))
            {
                //deve ser refeito
                _offsetFromTrackAnimated = Mathf.Lerp(_offsetFromTrackAnimated, (pos.Count - index - 1)*Offset, Time.deltaTime*Speed);
                
                //throw new NotImplementedException();

                _offsetFromTrack = Vector3.forward*_offsetFromTrackAnimated;
            }

            _counter++;
            
            if (_counter%60 == 0 && networkType == NetworkInstanceType.ListeningClient)
            {
                //throw new NotImplementedException();
                //_rankingController.RequestScoreFromServer();
            }
#endif

            base.OnUpdate();
        }

        protected override void ResetView()
        {
            base.ResetView();

            StartSelfDestruction();
        }


        protected override void StartSelfDestruction()
        {
            SetEndAnimation(true);
            base.StartSelfDestruction();

            /*
            if (_rankingController.PlayersPositions.Exists(x => x.id == Id))
            {
                int index = _rankingController.PlayersPositions.FindIndex(x => x.id == Id);

                SetEndAnimation(index == 0);
            }

            transform.DOLocalRotate(EndRotation, RotationDuration).SetEase(Ease.InOutCubic).Play();

            base.StartSelfDestruction();
            /**/
        }

        protected override void Destroyed()
        {
            _controller.Reset -= ResetView;
            base.Destroyed();
        }
    }
}