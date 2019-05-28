
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Views;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Page;
using UnityEngine;

namespace Assets.Scripts.Views.HUD.Scoreboard
{
    [RequireComponent(typeof(Page))]
    public class ScoreboardAutoRequest : DoOnPageAuto
    {
        private NetworkClientController _clientController;
        ScoreController _scoreController;

        protected override void OnStart()
        {
            base.OnStart();

            _clientController = _bootstrap.GetController(ControllerTypes.NetworkClient) as NetworkClientController;
            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
        }

        protected override void DoSomething()
        {
            _scoreController.RequestScoreboard();
        }
    }
}