using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class RankingTimeText : TextView
    {
        private RankingController _rankingController;

        [Space]
        public bool UseHardId;
        public string Id;

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
                _tx.text = PlayersPositions[Index].time.ToString("N");
        }
    }
}