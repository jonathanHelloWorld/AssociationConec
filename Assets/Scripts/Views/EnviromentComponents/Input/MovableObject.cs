using System;
using DG.Tweening;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.Input
{
    [RequireComponent(typeof (BoxCollider))]
    [RequireComponent(typeof (PanGesture))]
    [RequireComponent(typeof (Transformer2D))]
    [RequireComponent(typeof (Rigidbody))]
    public class MovableObject : GenericView
    {
        public int FloatToUpdate;
        public bool Normalize;
        public Vector3 MaxDistance;

        protected InputController _inputController;

        protected BoxCollider bx;
        protected Vector3 _iniPos;
        protected Vector3 _iniEuler;
        protected PanGesture _pg;

        protected override void OnAwake()
        {
            base.OnAwake();
            _iniPos = transform.position;
            _iniEuler = transform.eulerAngles;
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;
            _inputController.AddFloatController();

            _pg = GetComponent<PanGesture>();
            _pg.Panned += Pan;
            _pg.PanCompleted += EndPan;
            _iniPos = transform.position;

            bx = GetComponent<BoxCollider>();
            bx.size = new Vector3(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height, 1);
        }

        protected override void OnDestroied()
        {
            base.OnDestroied();
        }

        protected override void ResetView()
        {
            transform.DOPause();

            base.ResetView();
        }

        protected virtual void Pan(object sender, EventArgs e)
        {
            var sub = (transform.position - _iniPos);
            float distance;
            if (MaxDistance.magnitude > 0)
                distance = new Vector3(
                    sub.x/ (MaxDistance.x != 0 ? MaxDistance.x : 1) * (MaxDistance.x != 0 ? 1 : 0), 
                    sub.y/ (MaxDistance.y != 0 ? MaxDistance.y : 1) * (MaxDistance.x != 0 ? 1 : 0), 
                    sub.z/ (MaxDistance.z != 0 ? MaxDistance.z : 1) * (MaxDistance.x != 0 ? 1 : 0)
                    ).magnitude;
            else
                distance = Vector3.Distance(transform.position, _iniPos);
            
            if (Normalize)
                distance = distance > 1 ? 1 : distance < 0 ? 0 : distance;
            
            _inputController.FloatControllers[FloatToUpdate] = distance;
        }
        protected virtual void EndPan(object sender, EventArgs e)
        {

        }

        protected virtual void Associated() { }

        void Return()
        {
            GetComponent<RectTransform>()
                .DOMove(_iniPos, 0.3f)
                .SetEase(Ease.OutCubic)
                .Play();
        }

        void OnTriggerEnter(Collider other)
        {

        }
        void OnTriggerExit(Collider other)
        {

        }

        void Started()
        {

        }

        void Ended()
        {

        }
    }
}