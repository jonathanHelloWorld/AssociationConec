using System;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Run;
using InterativaSystem.Views.ControllableCharacters;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class PoolingObstacle : PoolingObject
    {
        public int ScoreDamage;

        private ScoreController _scoreController;
        private RunObstaclesPullingController _obstaclesPulling;

        protected override void OnStart()
        {
            base.OnStart();

            _obstaclesPulling = _bootstrap.GetController(ControllerTypes.ObstaclePulling) as RunObstaclesPullingController;
            _obstaclesPulling.ResetDependencies += Reset;

            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
        }

        protected override void OnCollect(GameObject other)
        {
            if (other.GetComponent<RunCharacter>() != null)
            {
                other.GetComponent<RunCharacter>().OnObstacleColision();
            }

            if (_scoreController == null) _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            if(_scoreController) _scoreController.AddScore(-ScoreDamage);
        }

        protected override void PlaySound()
        {
            if (_sfxController != null)
                _sfxController.PlaySound("Obstacle");
        }
    }
}