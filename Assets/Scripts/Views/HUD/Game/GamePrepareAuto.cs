using System.Collections;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Interfaces;
using InterativaSystem.Views;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Page;
using UnityEngine;

namespace Assets.Scripts.Views.HUD.Game
{
    [RequireComponent(typeof(Page))]
    public class GamePrepareAuto : DoOnPageAuto
    {
        public ControllerTypes gameControllerType;

        IController _gameController;

        protected override void OnStart()
        {
            base.OnStart();

            _gameController = _bootstrap.GetController(gameControllerType);
        }

        protected override void DoSomething()
        {
            _gameController.PrepareGame();
        }
    }
}