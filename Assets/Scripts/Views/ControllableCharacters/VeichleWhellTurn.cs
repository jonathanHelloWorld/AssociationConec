using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Run;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters
{
    public class VeichleWhellTurn : GenericView
    {
        private Vector3 _direction, _lastPosition, _forward;

        public float Angle = 30, TurnSpeed = 100;

        public bool CalculateFromInputAccelerometer = true;

        private InputController _inputController;

        protected override void OnStart()
        {
            _lastPosition = transform.position;

            _forward = transform.TransformDirection(Vector3.forward);

            _inputController = _bootstrap.GetController(ControllerTypes.Input) as InputController;
            _inputController.OnDeviceTurn += Turn;
        }

        private void Turn(Vector3 value)
        {
            _direction = value;
            Debug.Log(_direction);
            transform.localEulerAngles = new Vector3(0, _direction.x * Angle, 0);
        }

        void CalculateFromTransform()
        {
            var dir = transform.position - _lastPosition;
            var mag = dir.magnitude;
            dir.Normalize();

            dir -= Vector3.Scale(dir, _forward);
            dir.Normalize();

            _direction = (-_direction + dir) * Time.deltaTime * mag * TurnSpeed;

            _lastPosition = transform.position;

            transform.localEulerAngles = new Vector3(_direction.y * Angle, _direction.x * Angle, 0);
        }

        void FixedUpdate()
        {
            if (CalculateFromInputAccelerometer) return;

            CalculateFromTransform();
        }
    }
}