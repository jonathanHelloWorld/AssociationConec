using System;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using System.Linq;

namespace InterativaSystem.Views.HUD
{
    public class ScoreboardScoreText : TextView
    {
        private ScoreController _scoreController;

        public int Index;
        public int Type;

        protected override void OnStart()
        {
            base.OnStart();

            _scoreController = _controller as ScoreController;
            _scoreController.OnUpdateScoreboard += UpdateData;
        }

        private void UpdateData(List<ScoreValue> scoreboardData)
        {
            base.OnUpdate();

            List<ScoreValue> list = scoreboardData.FindAll(x => x.type == Type);
            list.OrderByDescending(x => x.value).ThenBy(x => x.time);

            if (list.ElementAtOrDefault(Index) != null)
                _tx.text = list[Index].value.ToString("000");
        }
    }
}