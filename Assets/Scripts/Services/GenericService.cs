using InterativaSystem.Controllers;
using InterativaSystem.Interfaces;
using UnityEngine;

namespace InterativaSystem.Services
{
    public class GenericService : MonoBehaviour
    {
        [HideInInspector]
        public ServicesTypes Type;
        public ControllerTypes ControllerType;

        protected IController _controller;
        protected ConsoleController _console;
        protected Bootstrap _bootstrap;

#if HAS_SERVER
#endif

        void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }

        void Start()
        {
            //the child needs to define a type for the controller
            if (Type == ServicesTypes.Null)
            {
                Debug.LogError("Type not set");
                return;
            }
            _bootstrap = Bootstrap.Instance;
            _bootstrap.SetService(this);

            _bootstrap.InitializeControllers += Initialize;

            //IsServer = this.GetType() == typeof(NetworkServerController);
        }

        private void Initialize()
        {
            OnStart();
        }

        protected void DebugLog(string debug)
        {
            if (_console == null)
            {
                Debug.Log(debug);
                return;
            }

            _console.Print(debug);
        }
        protected virtual void OnStart()
        {
            _controller = _bootstrap.GetController(ControllerType);
        }
    }
}