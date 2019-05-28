using InterativaSystem.Controllers;
using System.Collections;
using System.Collections.Generic;
using InterativaSystem.Interfaces;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Page
{
    public class PageCallActionAuto : DoOnPageAuto
    {
        public ControllerTypes gameControllerType;
        public int Id;

        IController _gameController;

        protected override void OnStart()
        {
            base.OnStart();
            _gameController = _bootstrap.GetController(gameControllerType);
        }

        protected override void DoSomething()
        {
            _gameController.CallAction(Id);
        }
    }
}
