using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Ranking
{
    public class RankingScoreText : TextView
    {
        private RankingController _rankingController;
        
        [Space]
        public int Index;

        protected override void OnStart()
        {
            base.OnStart();
            _rankingController = _controller as RankingController;
            _rankingController.OnScoreUpdate += UpdateText;
        }

        protected void UpdateText(List<ScoreValue> PlayersPositions)
        {
            if (PlayersPositions.Count > Index)
                _tx.text = PlayersPositions[Index].value.ToString("000");
        }
    }
}