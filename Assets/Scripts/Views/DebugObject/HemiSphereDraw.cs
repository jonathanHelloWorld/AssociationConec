using System;
using UnityEngine;

namespace InterativaSystem.Views.DebugObject
{
    public class HemiSphereDraw : MonoBehaviour
    {
        public float Radius = 2;
        [Range(0,180)]
        public float Angle = 30;

        void DrawHemiSphere(float radius, float angle)
        {
            var fwd = transform.TransformDirection(Vector3.forward);

            UnityEngine.Debug.DrawRay(transform.position, fwd * radius, Color.green);

            var x = (float)(radius * Math.Sin(angle * Math.PI / 180));
            var y = (float)(radius * Math.Cos(angle * Math.PI / 180));
            
            UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(-1*x, 0, y)), Color.red);
            UnityEngine.Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(x, 0, y)), Color.red);
        }

        void Update()
        {
            DrawHemiSphere(Radius, Angle);
        }
    }
}