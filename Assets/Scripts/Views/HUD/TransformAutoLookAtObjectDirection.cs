using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class TransformAutoLookAtObjectDirection : GenericView
    {
        public Transform lookTarket;

        Transform _transform;
        Vector3 lookTo;

        protected override void OnStart()
        {
            base.OnStart();

            _transform = GetComponent<Transform>();
        }

        protected override void OnLateUpdate()
        {
            base.OnLateUpdate();

            lookTo = lookTarket.position;
            lookTo.y = _transform.position.y;

            _transform.LookAt(lookTo);
        }
    }
}