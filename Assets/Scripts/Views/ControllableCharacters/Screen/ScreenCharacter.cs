using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;
using InterativaSystem.Views.GenericGame;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters.Screen
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class ScreenCharacter : GenericView
    {
        [Header("Settings")]
        public float Speed;
        public float BorderOffset;
        public float CameraDistance;

        private SFXController _sfx;

        protected DynamicQuizPointBased _quizPointBased;

        protected InputController _inputController;
        protected ScreenInfo _screen;

        //private Rigidbody _rigidbody;
        //private CapsuleCollider _collider;

        protected float _cameraDistance;
        protected Bounds _bounds;

        public Vector3 DisplaceDirection;

        private Vector3 _offsetDirection;
        private Vector3 _ini;

        public Animator[] CharController;

        protected override void OnStart()
        {
            _quizPointBased = _controller as DynamicQuizPointBased;
            _quizPointBased.OnFirstFaseEnd += EaseEndAnimation;
            _quizPointBased.OnGameEnd += GameEndAnimation;

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;

            _inputController = _bootstrap.GetController(ControllerTypes.Input) as InputController;
            _screen = _bootstrap.GetModel(ModelTypes.Screen) as ScreenInfo;

            //_rigidbody = GetComponent<Rigidbody>();
            //_collider = GetComponent<CapsuleCollider>();

            CalculateCameraDistance();
            _bounds = CalculateBounds();

            _inputController.OnPressMult += PressMove;
            _inputController.OnReleasesMult += ReleaseMove;

            _offsetDirection = Vector3.zero;

            _ini = transform.position;
        }

        private void GameEndAnimation()
        {
            for (int i = 0; i < CharController.Length; i++)
            {
                if (CharController[i] != null)
                {
                    CharController[i].SetBool("FaseEnd", false);
                }
            }

            if (_sfx != null)
                _sfx.PlaySound("GameEnd");
        }

        private void EaseEndAnimation()
        {
            for (int i = 0; i < CharController.Length; i++)
            {
                if (CharController[i] != null)
                {
                    Debug.Log("FaseEnd");
                    CharController[i].SetBool("FaseEnd", true);
                }
            }

            if (_sfx != null)
                _sfx.PlaySound("FaseEnd");
        }

        protected override void ResetView()
        {
            base.ResetView();

            transform.position = _ini;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            ActvateChar();
            UpdateMove();
        }

        void ActvateChar()
        {
            for (int i = 0; i < CharController.Length; i++)
            {
                CharController[i].gameObject.SetActive(_quizPointBased.SelectedChar == i);
            }
        }
        private void UpdateMove()
        {
            var finalPos = transform.position + transform.TransformDirection(_offsetDirection * Speed * Time.deltaTime);

            if (CheckCameraBounds(finalPos)) return;

            transform.position = finalPos;
        }

        private void ReleaseMove(int value)
        {
            _offsetDirection = DisplaceDirection * 0;
        }

        private void PressMove(int value)
        {
            _offsetDirection = DisplaceDirection * (value == 0 ? -1 : 1);
        }

        protected virtual void CalculateCameraDistance()
        {
            //var camPos = _screen.GetCamera().position;
            //_cameraDistance = Vector3.Distance(new Vector3(camPos.x, 0, camPos.z), new Vector3(transform.position.x, 0, transform.position.z));
            _cameraDistance = CameraDistance;
        }

        protected Bounds CalculateBounds()
        {
            var bounds = new Bounds(transform.position, Vector3.zero);
            var childs = transform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < childs.Length; i++)
            {
                _bounds.Encapsulate(childs[i].bounds);
            }
            return bounds;
        }

        protected bool CheckCameraBounds(Vector3 finalPos)
        {
            var bl = _screen.GetCorner(CameraPoint.BottonLeft, _cameraDistance - _bounds.size.z / 2);
            var tr = _screen.GetCorner(CameraPoint.TopRight, _cameraDistance - _bounds.size.z / 2);

            return (finalPos.x + _bounds.size.x / 2 + BorderOffset > tr.x || finalPos.x - _bounds.size.x / 2 - BorderOffset < bl.x);
        }

        protected virtual void AddPoint() { }

        protected virtual void AddPoint(int points)
        {
            for (int i = 0; i < CharController.Length; i++)
            {
                if (CharController[i] != null)
                {
                    CharController[i].PlayInFixedTime("Action");
                }
            }

            if(_sfx != null)
                _sfx.PlaySound("Action");

            _quizPointBased.AddPoints(points);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<GenericInfoView>() != null)
            {
                AddPoint(other.GetComponent<GenericInfoView>().Value);
                Destroy(other.gameObject);
            }
        }
    }
}