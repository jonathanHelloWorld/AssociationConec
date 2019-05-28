using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Run;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters
{
    public class RunCharacter : GenericView
    {
        Transform _transform;

        protected InputController _inputController;
        protected RunController _runController;
        protected RunTracks _tracksInfo;
        protected ScreenInfo _screen;
        protected float _cameraDistance;

        public Animator animator;

        protected int actualTrack = 0;
        protected Vector3 _offsetFromTrack;
        protected Bounds _bounds;

        [Header("Settings")]
        public float Speed;
        public float BorderOffset;
        public float CameraDistance;

        public float DestroyDelay;

        protected override void OnStart()
        {
            _transform = GetComponent<Transform>();

            _inputController = _controller as InputController;

            _runController = _bootstrap.GetController(ControllerTypes.Run) as RunController;
            _runController.OnGameEnd += StartSelfDestruction;

            _tracksInfo = _bootstrap.GetModel(ModelTypes.Tracks) as RunTracks;

            _screen = _bootstrap.GetModel(ModelTypes.Screen) as ScreenInfo;

            CalculateCameraDistance();

            _bounds = new Bounds(transform.position, Vector3.zero);
            var childs = transform.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < childs.Length; i++)
            {
                _bounds.Encapsulate(childs[i].bounds);
            }

            SetIdleAnimation();
        }

        protected virtual void StartSelfDestruction()
        {
            _runController.OnGameEnd -= StartSelfDestruction;

            Invoke("DestroySelf", DestroyDelay);
        }

        protected virtual void DestroySelf()
        {
            _runController.OnGameEnd -= DestroySelf;
            Destroy(this.gameObject);
        }

        protected virtual void CalculateCameraDistance()
        {
            //var camPos = _screen.GetCamera().position;
            //_cameraDistance = Vector3.Distance(new Vector3(camPos.x, 0, camPos.z), new Vector3(transform.position.x, 0, transform.position.z));
            _cameraDistance = CameraDistance;
        }

        protected override void OnUpdate()
        {
            if (!_runController.IsGameRunning) return;

            FollowPath();
            AnimateSpeed();
        }

        protected void ChangeTrack(int dir)
        {
            actualTrack += dir;
        }
        public void SetTrack(int pos)
        {
            actualTrack = pos;
            transform.position = _tracksInfo.Tracks[actualTrack].GetPointOnTrack(_runController.DistanceTraveled);
        }

        protected override void OnDestroied()
        {
            base.OnDestroied();

            Destroyed();
        }
        protected virtual void Destroyed() { }

        protected void FollowPath()
        {
            _transform.position = _tracksInfo.Tracks[actualTrack].GetPointOnTrack(_runController.DistanceTraveled) + _offsetFromTrack;
        }

        public void OnObstacleColision()
        {
            ObstacleStumble();
            _runController.OnObstacleColision();
        }

        protected virtual void Controll(Vector3 value) { }

        protected void SetIdleAnimation()
        {
            if (animator == null) return;
            //var rnd = new System.Random();
            animator.speed = 1f;
            animator.SetInteger("IdleStance", Random.Range(1, 5));
        }
        protected void AnimateSpeed()
        {
            if (animator == null) return;
            animator.SetFloat("Speed", _runController.Speed / _runController.MaxSpeed);
            animator.speed = Mathf.Clamp(_runController.Speed / _runController.MaxSpeed, 0.5f, 1f) + 0.5f;
            //animator.speed = 1f;
        }
        protected void ObstacleStumble()
        {
            animator.speed = 1f;
            animator.SetTrigger("Stumble");
        }
        protected virtual void SetEndAnimation(bool win)
        {
            if (animator == null) return;

            animator.speed = 1;
            animator.SetTrigger(win ? "Win" : "Lose");
        }
    }
}