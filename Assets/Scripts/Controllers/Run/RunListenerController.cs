using System;
using InterativaSystem.Models;
using InterativaSystem.Views;
using InterativaSystem.Views.ControllableCharacters;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace InterativaSystem.Controllers.Run
{
    [AddComponentMenu("ModularSystem/Controllers/Run/Listener Controller")]
    public class RunListenerController : RunController
    {
        NetworkConnectionsData _networkData;

        protected override void OnStart()
        {
            base.OnStart();

            _networkData = _bootstrap.GetModel(ModelTypes.Network) as NetworkConnectionsData;
        }

        protected override void InstantiateCharacter()
        {
#if HAS_SERVER
            int track = 1;
            var characters = _characterSelection.GetSessionCharacters();
            for (int i = 0; i < characters.Count; i++)
            {
                if (_networkData.GetConnection(characters[i].uid).isGamePrepared)
                {
                    InstantiateCharacter(characters[i].id, track, characters[i].uid);
                    track++;
                }
            }
#endif
        }

        public override void EndGame()
        {
            if (_gameEnding) return;
            Debug.Log("Run End");
            _gameEnding = true;
            DOTween.To(x => Speed = x, FixedSpeed, 0, 3f);

            Invoke("TimedEnd", 3f);
        }

        void TimedEnd()
        {
            base.EndGame();
        }
    }
}