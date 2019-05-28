using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters.Camera
{
    public class PivotRotationFromPan : GenericView
    {
        private InputController _input;

        public float Speed;

        protected override void OnStart()
        {
            base.OnStart();

            _input = _controller as InputController;

            _input.OnPan += Rotate;
        }

        private void Rotate(Vector3 value)
        {
            var dir = Vector3.up;
            var dirValue = value.x + value.y + value.z;

            transform.Rotate(dir * Time.deltaTime * Speed * dirValue);
        }

        public void OnDrawGizmos()
        {
            var size = 3;
            Debug.DrawLine(transform.position, transform.position + Vector3.up *        size, Color.magenta);
            Debug.DrawLine(transform.position, transform.position + Vector3.down *      size, Color.magenta);
            Debug.DrawLine(transform.position, transform.position + Vector3.left *      size, Color.magenta);
            Debug.DrawLine(transform.position, transform.position + Vector3.right *     size, Color.magenta);
            Debug.DrawLine(transform.position, transform.position + Vector3.forward *   size, Color.magenta);
            Debug.DrawLine(transform.position, transform.position + Vector3.back *      size, Color.magenta);
        }
    }
}