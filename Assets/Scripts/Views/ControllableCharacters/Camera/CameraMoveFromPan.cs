using System.Runtime.InteropServices;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters.Camera
{
    public class CameraMoveFromPan : GenericView
    {
        private InputController _input;

        public float Speed;

        protected override void OnStart()
        {
            base.OnStart();

            _input = _controller as InputController;

            _input.OnPan += Move;
        }

        private void Move(Vector3 value)
        {
            var dir = new Vector3(value.x, 0, value.y);
            transform.Translate(dir * Time.deltaTime * Speed);
        }
    }
}