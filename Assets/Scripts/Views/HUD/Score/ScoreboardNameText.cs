using System;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using System.Linq;

namespace InterativaSystem.Views.HUD
{
    public class ScoreboardNameText : TextView
    {
        ScoreController _scoreController;
        RegisterController _register;

        public int Index;
        public int Type;
        public string RegisterName = "Nome";

        protected override void OnStart()
        {
            base.OnStart();

            _scoreController = _controller as ScoreController;
            _scoreController.OnUpdateScoreboard += UpdateData;

            _register = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;
        }

        private void UpdateData(List<ScoreValue> scoreboardData)
        {
            base.OnUpdate();
            string text = "";

            List<ScoreValue> list = scoreboardData.FindAll(x => x.type == Type);
            list.OrderByDescending(x => x.value).ThenBy(x => x.time);

            if (list.ElementAtOrDefault(Index) != null)
            {
                _register.TryGetRegistryValue(list[Index].uid, RegisterName, out text);
                _tx.text = text;
            }
        }
    }
}