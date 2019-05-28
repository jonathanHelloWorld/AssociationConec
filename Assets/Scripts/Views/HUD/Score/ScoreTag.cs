using UnityEngine;
using System.Collections;
using InterativaSystem.Controllers;
using System.Collections.Generic;
using InterativaSystem.Models;
using UnityEngine.UI;
using Assets.Scripts.Views.HUD;

namespace InterativaSystem.Views.HUD.Network
{
    public class ScoreboardScoreText : DynamicText
    {
        ScoreController _scoreController;

        public int position = 0;
        public Text scoreText;

        protected override void OnStart()
        {
            base.OnStart();

            _scoreController = _controller as ScoreController;
        }

        void UpdateScore(List<ScoreValue> scores)
        {

        }
    }
}
