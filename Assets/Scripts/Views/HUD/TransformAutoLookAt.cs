using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class TransformAutoLookAt : GenericView
    {
        public Transform Target;

        public Transform RootParent;

        public Transform CanvasRoot;

        protected override void OnLateUpdate()
        {
            base.OnUpdate();
            //transform.LookAt(Target);
            if (Target == null || RootParent == null) return;

            transform.LookAt(Target, RootParent.TransformDirection(Vector3.back));



            //transform.localEulerAngles += Vector3.up*90;
            transform.Rotate(Vector3.right, 90, Space.Self);
            transform.Rotate(Vector3.forward, -90, Space.Self);
            //transform.Rotate(transform.TransformDirection(Vector3.forward), 90);


            var distance = Vector3.Distance(transform.position, Target.transform.position);
            transform.localScale = new Vector3(distance / CanvasRoot.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}