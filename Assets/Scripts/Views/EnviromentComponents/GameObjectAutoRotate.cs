using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class GameObjectAutoRotate : GenericView
    {
        public Vector3 Rotation;

        public bool CanRotate;

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (!CanRotate) return;

            this.transform.Rotate(Rotation * Time.deltaTime, Space.World);
        }

        protected override void ResetView()
        {
            base.ResetView();

            CanRotate = true;
        }
    }
}