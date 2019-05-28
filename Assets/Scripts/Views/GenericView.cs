using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Interfaces;
using UnityEngine;

namespace InterativaSystem.Views
{
    public class GenericView : MonoBehaviour
    {
        [SerializeField]
        protected ControllerTypes _controllerType;
        protected IController _controller;
        protected SoundController _sfxController;
        protected TimeController _timeController;
        protected Bootstrap _bootstrap;

        public void SetControllerType(ControllerTypes type)
        {
            _controllerType = type;
        }
        public ControllerTypes GetControllerType()
        {
            return _controllerType;
        }

        void Awake()
        {
            OnAwake();
        }
        void Start()
        {
            _bootstrap = Bootstrap.Instance;

            _bootstrap.InitializeViews += Initialize;
        }
        void Update()
        {
            if (!_bootstrap.IsAppRunning)
                return;

            OnUpdate();
        }

        private void LateUpdate()
        {
            if (!_bootstrap.IsAppRunning)
                return;

            OnLateUpdate();
        }

        public void OnDestroy()
        {
            OnDestroied();

            if (_bootstrap != null)
                _bootstrap.InitializeViews -= Initialize;

            if (_controller != null)
                _controller.Reset -= ResetView;
        }

        public void Initialize()
        {
            if (_bootstrap == null)
                _bootstrap = Bootstrap.Instance;

            if (_controllerType != ControllerTypes.Null) 
            {
                _controller = _bootstrap.GetController(_controllerType);
                if (_controller != null) 
                    _controller.Reset += ResetView;
            }

            _sfxController = _bootstrap.GetController(ControllerTypes.SoundSFX) as SoundController;
            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;

            _bootstrap.Reset += ResetFromBootstrap;

            OnStart();
        }

        protected virtual void ResetFromBootstrap() { }
        protected virtual void ResetView() { }
        protected virtual void OnDestroied() { }
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
    }
}