using System;
using System.Collections;
using InterativaSystem.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterativaSystem.Models
{
    public enum CameraPoint
    {
        Center = 0,
        BottonLeft = 1,
        TopRight = 2
    }
    [AddComponentMenu("ModularSystem/InfoModel/ Screen Info")]
    public class ScreenInfo : GenericModel
    {
        private Transform _cam;
        private Camera _Gamcam;

        public Vector2 Resolution;

        [Header("Camera point and click parameters")]
        public bool usePointAndClick;
        public LayerMask mask;
        Button actuaButton;
        public float ClickDelay;
        public float ClickTimeLapse;

        #region MonoBehaviour Methods
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Screen;

            if (Camera.main != null)
            {
                _cam = Camera.main.transform;
                _Gamcam = Camera.main;
            }
        }

        protected override void OnStart()
        {
            _bootstrap.AppStarted += GetSceneCamera;
            _bootstrap.GameControllerStarted += GetSceneCamera;

            if (usePointAndClick)
                StartCoroutine(TriggerClick());
        }

        void Update()
        {
            if (usePointAndClick)
            {
                CenterPointTrace();
            }
        }
        #endregion

        public Vector3 GetCorner(CameraPoint corner)
        {
            return GetCorner(corner, 1);
        }
        public Vector3 GetCorner(CameraPoint corner, float distance)
        {
            var pos = new Vector3();

            var fov = _cam.GetComponent<Camera>().fieldOfView * Math.PI / 180;
            float aspect = (float)Screen.width / (float)Screen.height;
            
            switch (corner)
            {
                case CameraPoint.Center:
                    pos = _cam.transform.position + _cam.transform.TransformDirection(Vector3.forward*distance);
                    break;
                case CameraPoint.BottonLeft:
                    pos = _cam.transform.position + _cam.transform.TransformDirection(
                        Vector3.forward * distance +
                        (Vector3.left * (float)(Math.Tan(fov / 2) * distance * aspect)) +
                        (Vector3.down * (float)(Math.Tan(fov / 2) * distance))
                        );
                    break;
                case CameraPoint.TopRight:
                    pos = _cam.transform.position + _cam.transform.TransformDirection(
                        Vector3.forward * distance +
                        (Vector3.right * (float)(Math.Tan(fov / 2) * distance * aspect)) +
                        (Vector3.up * (float)(Math.Tan(fov / 2) * distance))
                        );
                    break;
            }

            if(_bootstrap.IsEditor)
                Debug.DrawRay(pos, Vector3.up*2,Color.red,5);
            return pos;
        }

        public void GetSceneCamera()
        {
            if (_cam == null && Camera.main != null)
                _cam = Camera.main.transform;
        }
        public Transform GetCamera()
        {
            return _cam;
        }
        public void SetCamera(Transform cam)
        {
            _cam = cam;
            _Gamcam = cam.GetComponent<Camera>();
        }

        public Vector3 GetWordPosToScreen(Transform obj)
        {
            Vector2 pos;

            pos = _Gamcam.WorldToViewportPoint(obj.position);
            
            pos = new Vector2(pos.x * Resolution.x - Resolution.x / 2, pos.y * Resolution.y - Resolution.y / 2);

            return pos;
        }

        public Texture2D TakeScreenshot()
        {
            Debug.LogError("Not implemented");
            return null;
        }

        void CenterPointTrace()
        {
            var fwd = transform.TransformDirection(Vector3.forward);
            RaycastHit other;
            var ray = new Ray(transform.position, fwd);

            if (Physics.Raycast(ray, out other,  1000, mask))
            {
                EndPoint = ray.GetPoint(other.distance*0.8f);
                actuaButton = other.transform.gameObject.GetComponent<Button>();

                Debug.DrawRay(ray.origin, ray.direction * other.distance, Color.red);

                if (actuaButton && singleClick)
                {
                    singleClick = false;
                    ClickTimeLapse = 0f;
                }
            }
            else
            {
                EndPoint = ray.GetPoint(1000);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            }
        }

        [HideInInspector]
        public Vector3 FillPos, EndPoint;

        private bool singleClick;
        IEnumerator TriggerClick()
        {
            while (true)
            {
                while (!singleClick && ClickTimeLapse < ClickDelay && actuaButton)
                {
                    FillPos = actuaButton.transform.position;

                    yield return null;
                    ClickTimeLapse += Time.deltaTime;
                }

                if (actuaButton != null)
                {
                    actuaButton.onClick.Invoke();
                    yield return new WaitForSeconds(ClickDelay);
                    actuaButton = null;
                }

                ClickTimeLapse = 0;
                singleClick = true;
                yield return null;
            }
        }
    }
}