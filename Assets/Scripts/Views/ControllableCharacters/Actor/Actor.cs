using System.Collections.Generic;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters.Actor
{
    public class Actor : GenericView
    {
        protected InputController inputController;

        public List<ControlerButton> AcceptableButtons;

        public int ControllerIndex = 0;

        protected override void OnStart()
        {
            base.OnStart();

            inputController = _controller as InputController;
            
#if HAS_CONTROLLERINPUT
            inputController.OnJoystickPrees += Presses;
            inputController.OnJoystickRelease += Releases;
            inputController.OnJoystickAxis += Axis;
#endif
        }

        private void Presses(ControlerButton button, int index)
        {
            if (ControllerIndex != index) return;
            if (!AcceptableButtons.Contains(button)) return;

            OnPress(button);
        }

        private void Releases(ControlerButton button, int index)
        {
            if (ControllerIndex != index) return;
            if (!AcceptableButtons.Contains(button)) return;

            OnRelease(button);
        }

        private void Axis(ControlerButton button, Vector2 axis, int index)
        {
            if (ControllerIndex != index) return;
            if (!AcceptableButtons.Contains(button)) return;

            OnAxis(button, axis);
        }

        protected virtual void OnPress(ControlerButton button) { }
        protected virtual void OnRelease(ControlerButton button) { }
        protected virtual void OnAxis(ControlerButton button, Vector2 axis) { }
    }
}