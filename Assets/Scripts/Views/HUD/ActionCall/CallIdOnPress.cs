using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallIdOnPress : GenericView
    {
        public int IdToCall, IdToCheck;
        private InputController _inputController;
        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;
            _inputController.OnPressMult += CallId;
        }

        private void CallId(int value)
        {
            if (IdToCheck != value) return;

            _controller.CallAction(IdToCall);
        }
    }
}