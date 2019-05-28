using System;
using System.Collections.Generic;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.HUD;
using System.Linq;

namespace Assets.Scripts.Views.HUD.Scoreboard
{
    public class ScoreboardTimeText : TextView
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
            {
                TimeSpan ts = TimeSpan.FromSeconds(list[Index].time);
                _tx.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            }
        }
    }
}