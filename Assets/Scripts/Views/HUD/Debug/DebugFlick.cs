using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Debug
{
    public class DebugFlick : GenericView
    {
        private InputController _inputController;
        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;
            _inputController.OnFlick += Debug;
        }

        private void Debug(Vector3 value)
        {
            UnityEngine.Debug.Log(value);
        }
    }
}