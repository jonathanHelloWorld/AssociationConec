using System.Collections;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Views;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Page;
using UnityEngine;

namespace Assets.Scripts.Views.HUD.Game
{
    public class GameResetButton : ButtonView
    {
        protected override void OnClick()
        {
            base.OnClick();

           _controller.ResetGame();
        }
    }
}