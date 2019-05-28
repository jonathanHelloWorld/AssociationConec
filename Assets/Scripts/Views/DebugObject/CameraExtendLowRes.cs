using System;
using UnityEngine;

namespace InterativaSystem.Views.DebugObject
{
    [RequireComponent(typeof(Camera))]
    public class CameraExtendLowRes : GenericView
    {
        private Camera _camera, _cameraExtended;
        private Transform _renderPlane;

        public RenderTexture RenderTarget;

        public LayerMask CullingMask;
        private float _aspect;
        private float _alpha;

        protected override void OnAwake()
        {
            base.OnAwake();

            _camera = GetComponent<Camera>();
            var temp = new GameObject("Extended Camera");
            _cameraExtended = temp.AddComponent<Camera>();

            _cameraExtended.enabled = false;

            _cameraExtended.gameObject.AddComponent<GUILayer>();
            _cameraExtended.gameObject.AddComponent<FlareLayer>();

            _cameraExtended.fieldOfView = _camera.fieldOfView;
            _cameraExtended.nearClipPlane = 0;
            _cameraExtended.nearClipPlane = _camera.farClipPlane;
            _cameraExtended.farClipPlane = _camera.farClipPlane * 4;
            _cameraExtended.cullingMask = CullingMask;
            _cameraExtended.clearFlags = _camera.clearFlags;
            _cameraExtended.backgroundColor = _camera.backgroundColor;

            _cameraExtended.targetTexture = Resources.Load<RenderTexture>("ExtendableTexture");
        }

        protected override void OnStart()
        {
            base.OnStart();

            var distance = _camera.farClipPlane - 0.1f;

            _cameraExtended.transform.parent = _camera.transform;
            _cameraExtended.transform.localEulerAngles = Vector3.zero;

            _cameraExtended.transform.localPosition = Vector3.zero;

            var temp = Instantiate(Resources.Load<GameObject>("Extendable Plane"));
            _renderPlane = temp.transform;
            _renderPlane.parent = _camera.transform;
            _renderPlane.localEulerAngles = Vector3.zero;

            _renderPlane.localPosition = new Vector3(0, 0, distance);

            _alpha = (float)((_camera.fieldOfView * Math.PI / 180) / 2);
            _aspect = (float)Screen.width / (float)Screen.height;
            Debug.Log(_aspect);

            var side = (float) (Math.Tan(_alpha)*distance);

            var points = new Vector3[4];
            points[0] = new Vector3(side * _aspect, side);
            points[1] = new Vector3(side * _aspect, side*-1);
            points[2] = new Vector3(side * _aspect*-1, side);
            points[3] = new Vector3(side * _aspect*-1, side*-1);

            var mesh = _renderPlane.GetComponent<MeshFilter>();
            var vertex = mesh.mesh.vertices;

            vertex[0] = points[3];
            vertex[1] = points[0];
            vertex[2] = points[1];
            vertex[3] = points[2];
            
            mesh.mesh.vertices = vertex;

            _cameraExtended.enabled = true;
        }
    }
}