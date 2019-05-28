using UnityEngine;

namespace InterativaSystem.Views.DebugObject
{
    public class DrawArcRay : MonoBehaviour
    {
        public float Radius;
        public int Definition;

        public Vector3 Up = Vector3.up;
        public Vector3 Forward = Vector3.forward;

        void Start()
        {
        }

        public void OnDrawGizmos()
        {
            DrawArc();
        }
        void DrawArc()
        {
            _center = transform.position;
            Up = transform.TransformDirection(Vector3.up);
            Forward = transform.TransformDirection(Vector3.forward);
            for (int i = 0; i < Definition; i++)
            {
                Debug.DrawLine(_center + GetPointOnCircle((float)i / (float)(Definition) * 360), _center+GetPointOnCircle((float)(i + 1) / (float)(Definition) * 360), Color.red);
            }
        }

        private Vector3 _center;
        Vector3 GetPointOnCircle(float angle)
        {
            //angle = angle * Mathf.PI / 180;

            /*
            point = new Vector3(
                point.x * Mathf.Cos(angle) - point.z * Mathf.Sin(angle),
                0,
                point.z * Mathf.Cos(angle) - point.x * Mathf.Sin(angle)
                );
            */
            return Quaternion.Euler(Up * angle) * Forward * Radius;
        }
    }

    public class Arc
    {
        
    }
}