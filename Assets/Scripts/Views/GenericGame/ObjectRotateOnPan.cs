using System;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.GenericGame
{
    public class ObjectRotateOnPan : PanView
    {
        public Vector3 RotationAxis;
        public float Speed;
        protected override void OnStart()
        {
            base.OnStart();

            _panGesture.Panned += Rotate;
        }

        private void Rotate(object sender, EventArgs e)
        {
            var delta = _panGesture.LocalDeltaPosition;
            var magnitute = delta.magnitude;

            var point = _panGesture.WorldTransformCenter;
            var center = transform.position;

            var side = delta.x + delta.y;

            //Above
            if (point.x > center.x)
            {
                //Right
                if (point.y > center.y)
                {
                    if (side > 0 && delta.x > delta.y ||
                        side < 0 && delta.x > delta.y
                        )
                        magnitute *= -1;
                }
                //Left
                else
                {
                    if (side < 0)
                        magnitute *= -1;
                }
            }
            //Bellow
            else
            {
                //Right
                if (point.y > center.y)
                {
                    if (side > 0)
                        magnitute *= -1;
                }
                //Left
                else
                {
                    if (side < 0 && delta.x < delta.y ||
                        side > 0 && delta.x < delta.y
                        )
                        magnitute *= -1;
                }
            }

            transform.Rotate(RotationAxis, magnitute*Speed*Time.deltaTime);
        }
    }
}