using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InterativaSystem.Views.ControllableCharacters
{
    public class RunCharacterTurn : RunCharacter
    {
        protected override void OnStart()
        {
            base.OnStart();

            _inputController.OnDeviceTurn += Controll;
        }

        protected override void Controll(Vector3 value)
        {
            if (!_runController.IsGameRunning) return;

            var movementDir = Vector3.right * Speed * Time.deltaTime * value.x;

            var finalPos = transform.position + transform.TransformDirection(movementDir);
            var bl = _screen.GetCorner(CameraPoint.BottonLeft, _cameraDistance - _bounds.size.z / 2);
            var tr = _screen.GetCorner(CameraPoint.TopRight, _cameraDistance - _bounds.size.z / 2);

            if (finalPos.x + _bounds.size.x / 2 + BorderOffset > tr.x)
                movementDir.x -= Mathf.Abs(movementDir.x);
            if (finalPos.x - _bounds.size.x / 2 - BorderOffset < bl.x)
                movementDir.x += Mathf.Abs(movementDir.x);

            _offsetFromTrack = _offsetFromTrack + transform.TransformDirection(movementDir);
        }
        protected override void ResetView()
        {
            base.ResetView();

            base.StartSelfDestruction();
        }


        protected override void StartSelfDestruction()
        {
            SetEndAnimation(true);
            base.StartSelfDestruction();
        }

        protected override void DestroySelf()
        {
            _controller.Reset -= ResetView;
            _inputController.OnDeviceTurn -= Controll;
            base.DestroySelf();
        }

        protected override void Destroyed()
        {
            //_inputController.OnDeviceTurn -= Controll;

            base.Destroyed();
        }
    }
}