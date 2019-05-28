using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.GenericGame
{
    public class ObjectAutoTranlate : GenericView
    {
        public Vector3 Direction;
        public float Speed;

        private ScreenInfo _screen;
        public float _cameraDistance = 10;

        protected override void OnStart()
        {
            base.OnStart();

            _screen = _bootstrap.GetModel(ModelTypes.Screen) as ScreenInfo;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            transform.Translate(Direction * Speed * Time.deltaTime);

            if (!CheckCameraBounds(transform.position))
            {
                Destroy(this.gameObject);
            }
        }
        protected bool CheckCameraBounds(Vector3 finalPos)
        {
            var bl = _screen.GetCorner(CameraPoint.BottonLeft, _cameraDistance);
            var tr = _screen.GetCorner(CameraPoint.TopRight, _cameraDistance);

            return (
                finalPos.x < tr.x && finalPos.x > bl.x &&
                finalPos.y < tr.y && finalPos.y > bl.y
                );
        }
    }
}