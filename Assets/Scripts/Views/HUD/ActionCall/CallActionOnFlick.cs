using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallActionOnFlick : GenericView
    {
        public int Id;
        private InputController _inputController;
        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _bootstrap.GetController(ControllerTypes.Input) as InputController;
            _inputController.OnFlick += CallId;
        }

        private void CallId(Vector3 value)
        {
            _controller.CallAction(Id);
        }
    }
}