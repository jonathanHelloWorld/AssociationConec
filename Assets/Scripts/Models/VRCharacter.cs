using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InterativaSystem.Models;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;

namespace InterativaSystem.Models
{
    public class VRCharacter : GenericModel
    {
        VRCharacterController _vrCharacterController;
        SFXController _sfxController;

        Transform _transform;
        UnityEngine.AI.NavMeshAgent _navMesh;
        public Transform characterCamera;
        public string moveToTag = "MoveTo";
        public LayerMask mask;
        Button actuaButton;
        public float ClickDelay;
        public float ClickTimeLapse;
        public float minRange;
        public Transform startingPos;

        [HideInInspector]
        public Vector3 FillPos, EndPoint;

        float stepsTimer = 0f;
        bool isMoving = false;
        bool singleClick;
        //Todo Diego: Corrigir referencia
        //OVRScreenFade _fade; 

        GameObject moveTo;

        void Awake()
        {
            Type = ModelTypes.VRCharacter;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _vrCharacterController = _bootstrap.GetController(ControllerTypes.VRCharacter) as VRCharacterController;
            _vrCharacterController.Reset += OnReset;

            _sfxController = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;

            _transform = GetComponent<Transform>();
            _navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();

            //_fade = characterCamera.GetComponent<OVRScreenFade>();

            StartCoroutine(TriggerClick());
            StartCoroutine(PlayFootsteps());
        }

        void LateUpdate()
        {
            if (_vrCharacterController != null)
                CenterPointTrace();
        }

        void CenterPointTrace()
        {
            if(_vrCharacterController == null) _vrCharacterController = _bootstrap.GetController(ControllerTypes.VRCharacter) as VRCharacterController;

            //var fwd = characterCamera.TransformDirection(Vector3.forward);
            RaycastHit other;
            var ray = new Ray(characterCamera.position, characterCamera.forward);

            if (Physics.Raycast(ray, out other, Mathf.Infinity, mask))
            {
                //_text.text = other.collider.name;
                EndPoint = ray.GetPoint(other.distance * 0.8f);
                actuaButton = other.transform.gameObject.GetComponent<Button>();

                if (_vrCharacterController.IsGameStarted)
                {
                    GameObject go = null;
                    if (other.collider.CompareTag(moveToTag))
                        go = other.collider.gameObject;

                    if (go != null)
                    {
                        isMoving = true;

                        if (moveTo != go)
                        {
                            if (moveTo != null)
                                StopMove();

                            moveTo = go;
                        }

                        Vector3 v;
                        if (go.CompareTag(moveToTag))
                        {
                            v = other.point + (characterCamera.forward * 0.5f);
                            v.y = other.collider.transform.position.y;
                        }
                        else
                            v = go.transform.position;

                        Move(v);
                    }
                    else if (moveTo != null)
                    {
                        StopMove();
                        isMoving = false;
                        moveTo = null;
                    }
                }

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

        IEnumerator TriggerClick()
        {
            while (true)
            {
                //if (actuaButton != null && Vector3.Distance(_cam.position, actuaButton.transform.position) <= minRange)
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
                        actuaButton = null;
                        yield return new WaitForSeconds(ClickDelay);
                    }

                    ClickTimeLapse = 0;
                    singleClick = true;
                    yield return null;
                }

                //yield return null;
            }
        }

        public void SetPosition(Vector3 pos, Quaternion rot)
        {

            _navMesh.Warp(pos);
            //_transform.position = pos;
            _transform.rotation = rot;
        }

        public void Move(Vector3 pos)
        {
            if (_navMesh == null || !_navMesh.enabled) return;

            _navMesh.SetDestination(pos);
            _navMesh.Resume();
        }

        public void StopMove()
        {
            if (_navMesh != null && _navMesh.enabled)
                _navMesh.Stop();
        }

        public void Fade(bool _in, float delay, GenericController.SimpleEvent callback)
        {
            StopMove();
            //_fade.Fade(_in, delay, callback);
        }

        public void EnableNavMesh()
        {
            _navMesh.enabled = true;
        }

        void OnReset()
        {
            _transform.position = startingPos.position;
            _transform.rotation = startingPos.rotation;
            EnableNavMesh();
        }

        public Transform GetTransform() { return _transform; }

        IEnumerator PlayFootsteps()
        {
            while(true)
            {
                if (isMoving && _vrCharacterController.IsGameRunning)
                {
                    stepsTimer += Time.deltaTime;

                    if (stepsTimer >= 0.7f)
                    {
                        stepsTimer = 0f;
                        _sfxController.PlaySound("Footsteps");
                    }
                }

                yield return null;
            }
        }
    }
}