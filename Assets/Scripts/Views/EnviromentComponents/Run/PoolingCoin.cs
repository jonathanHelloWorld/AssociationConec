using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Run;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class PoolingCoin : PoolingObject
    {
        public int Score;
        public int type;
        
        private ScoreController _scoreController;
        private RunCoinsPullingController _coinsPulling;

        protected override void OnStart()
        {
            base.OnStart();

            _coinsPulling = _bootstrap.GetController(ControllerTypes.CoinPulling) as RunCoinsPullingController;
            _coinsPulling.ResetDependencies += Reset;

            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
        }
        protected override void OnCollect(GameObject other)
        {
            if (DoNothing) return;

            if (_scoreController == null) _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;

            if (_scoreController) _scoreController.AddScore(Score, 0, type);
        }
        protected override void PlaySound()
        {
            if (DoNothing) return;

            if (_sfxController != null)
                _sfxController.PlaySound("Coin");
        }
    }
}