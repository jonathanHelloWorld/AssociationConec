using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.DebugObject
{
    public class InputControllersDebug : GenericView
    {
        private InputController _inputController;
        public int ControllerIndex = 0;

#if HAS_CONTROLLERINPUT
        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;

            _inputController.OnJoystickPrees += Presses;
            _inputController.OnJoystickAxis += Axis;
        }

        private void Presses(ControlerButton button, int index)
        {
            if (ControllerIndex != index) return;

            Debug.Log("Button " + index + ": " + button);
        }

        private void Axis(ControlerButton button, Vector2 axis, int index)
        {
            if (ControllerIndex != index) return;

            Debug.Log("Axis " + index + ": " + axis);
        }
#endif
    }
}