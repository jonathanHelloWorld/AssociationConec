using UnityEngine;
using System.Collections;
using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;

namespace Interativa.Views.CandyGame
{
    public class CandyRemainingGelsText : DynamicText
    {
        CandyController _candyController;

        protected override void OnStart()
        {
            base.OnStart();

            _candyController = _controller as CandyController;
            _candyController.RemainingGelsText += UpdateData;
        }

        protected override void UpdateData(string value)
        {
            UpdateText(value);
        }
    }
}