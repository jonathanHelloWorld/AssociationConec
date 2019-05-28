using System;
using System.Linq;
using DG.Tweening;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Controllers
{
    public class GenericController : MonoBehaviour, IController
    {
        public delegate void SimpleEvent();
		public delegate void RegisterEvent(string DataName, string Value);
        public delegate void ObjectEvent(object obj);
        public delegate void IntEvent(int value);
        public delegate void BoolEvent(bool value);
        public delegate void IntDoubleEvent(int reference,int value);
        public delegate void FloatEvent(float value);
        public delegate void AxisEvent(Vector2 value);
        public delegate void VectorEvent(Vector3 value);
        public delegate void StringEvent(string value);

        public event SimpleEvent Reset, ResetDependencies, OnGameStart, OnGamePrepare, OnGamePause, OnGameResume, OnGameEnd, ObstacleCollision, OnInitializationEnd;
        public event IntEvent CallGenericAction , GenericActionEnded;

        [HideInInspector]
        public bool IsGameStarted { get; protected set; }
        public bool IsGameRunning { get; protected set; }

        [HideInInspector]
        public ControllerTypes Type { get; protected set; }
        [HideInInspector]
        public float GameTime { get; protected set; }

        protected ConsoleController _console;
        protected Bootstrap _bootstrap;
        protected Type _thisType;

        #region Network Parameters and Methods
#if HAS_SERVER

        [Header("Network")]
        public bool sendGameStateToServer = true;

        [Space(20f)]

        protected bool _isServer;
        protected NetworkClientController _clientController;
        protected NetworkServerController _serverController;

        public NetworkController GetNetworkController()
        {
            if(_isServer)
                return _serverController;

            return _clientController;
        }

        protected void CheckNetworCall(string method, string json)
        {
            if (Type == ControllerTypes.NetworkServer)
                return;
            if (Type == ControllerTypes.NetworkClient)
                return;

            var meth = _thisType.GetMethods().ToList();

            if (!meth.Exists(x => x.Name == method)) return;

            var param = new object[] { json };

            try
            {
                _thisType.GetMethod(method).Invoke(this, param);
                DebugLog("Called: " + method + ", on " + _thisType);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError(this + " - " + method + "\n\n" + e.ToString());
#endif
            }
            /**/
        }
        protected void CheckNetworCall(string id, string method, string json)
        {
            if (Type == ControllerTypes.NetworkServer)
                return;
            if (Type == ControllerTypes.NetworkClient)
                return;

            var meth = _thisType.GetMethods().ToList();

            if (!meth.Exists(x => x.Name == method)) return;

            var param = new object[] { id, json };

            try
            {
                _thisType.GetMethod(method).Invoke(this, param);
                DebugLog("Called: " + method + ", on " + _thisType);
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError(this + " - " + method + " - " + id + "\n\n" + e.ToString());
#endif
            }
            /**/
        }

        public void NetworkCallAction(string json)
        {
            var id = JsonConvert.DeserializeObject<int>(json);
            DebugLog("Calling " + id);

            if (CallGenericAction != null) CallGenericAction(id);
        }

        public void NetworkActionEnded(string json)
        {
            var id = JsonConvert.DeserializeObject<int>(json);

            DebugLog("Action Ended " + id);
            if (GenericActionEnded != null) GenericActionEnded(id);
        }

        public void BootstrapPrepareGame(string json)
        {
            var game = JsonConvert.DeserializeObject<ControllerTypes>(json);

            if (game == Type)
                PrepareGame();
        }
#endif
        #endregion

        #region Initialization
        void Start()
        {
            //the child needs to define a type for the controller
            if (Type == ControllerTypes.Null)
            {
                Debug.LogError("Type not set");
                return;
            }
            _bootstrap = Bootstrap.Instance;
            _bootstrap.SetController(this);

            _bootstrap.InitializeControllers += Initialize;

            //IsServer = this.GetType() == typeof(NetworkServerController);
        }
        void Initialize()
        {
            DebugLog(this + " Started");
            IsGameStarted = false;

            _bootstrap.Reset += CallReset;
            _bootstrap.AppStarted += CallReset;

            try
            {
                _console = _bootstrap.GetController(ControllerTypes.Console) as ConsoleController;
            }
            catch (Exception)
            {
                Debug.LogError("There is no Console Controller in the scene");
            }

            _thisType = GetType();

#if HAS_SERVER
            try
            {
                var server = _bootstrap.GetController(ControllerTypes.NetworkServer);
                _isServer = server != null;
            }
            catch (Exception)
            {
                throw;
            }
            if (!_isServer)
            {
                _clientController = _bootstrap.GetController(ControllerTypes.NetworkClient) as NetworkClientController;
                _clientController.OnReceive += CheckNetworCall;
            }
            else
            {
                _serverController = _bootstrap.GetController(ControllerTypes.NetworkServer) as NetworkServerController;
                _serverController.OnReceive += CheckNetworCall;
            }
#endif

            OnStart();
        }
        protected virtual void OnStart()
        {
            GetReferences();

            if (OnInitializationEnd != null) OnInitializationEnd();
        }

        /// <summary>
        /// Metodo usado para buscar referencias de outros Models ou Controllers
        /// </summary>
        protected virtual void GetReferences() { }
        #endregion

        #region Call Actions Methods
        public void CallAction(int id)
        {
            DebugLog("Calling " + id);
#if HAS_SERVER
            if(_isServer)
                _serverController.SendMessageToAll("NetworkCallAction", JsonConvert.SerializeObject(id));
            else
                _clientController.SendMessageToServer("NetworkCallAction", JsonConvert.SerializeObject(id));
#endif
            if (CallGenericAction != null) CallGenericAction(id);
        }
        public void ActionEnded(int id)
        {
            DebugLog("Action Ended " + id);
#if HAS_SERVER
            if (_isServer)
                _serverController.SendMessageToAll("NetworkActionEnded", JsonConvert.SerializeObject(id));
            else
                _clientController.SendMessageToServer("NetworkActionEnded", JsonConvert.SerializeObject(id));
#endif
            if (GenericActionEnded != null) GenericActionEnded(id);
        }
        #endregion

        #region Game manipulation methods
        public virtual void StartGame()
        {
#if HAS_SERVER
            if (!_isServer)
                _clientController.SendMessageToServer("NetworkGameStarted", "");
#endif
            IsGameStarted = true;
            IsGameRunning = true;

            if (OnGameStart != null) OnGameStart();
        }
        public virtual void PrepareGame()
        {
            Debug.Log("Game Prepare: " + Type.ToString());
#if HAS_SERVER
            if(!_isServer && sendGameStateToServer)
                _clientController.SendMessageToServer("NetworkGamePrepared", "");
#endif
            if (OnGamePrepare != null) OnGamePrepare();
        }
        public virtual void PauseGame()
        {
            IsGameRunning = false;
            if (OnGamePause != null) OnGamePause();
        }
        public virtual void ResumeGame()
        {
            IsGameRunning = true;
            if (OnGameResume != null) OnGameResume();
        }
        public virtual void EndGame()
        {
#if HAS_SERVER
            if (!_isServer && sendGameStateToServer)
                _clientController.SendMessageToServer("NetworkGameEnded", "");
#endif
            IsGameStarted = false;
            IsGameRunning = false;
            if (OnGameEnd != null) OnGameEnd();
        }
        public void ResetGame()
        {
            Debug.Log("Game Reset: " + Type.ToString());

            CallReset();
        }
        #endregion

        protected void DebugLog(string debug)
        {
            if (_bootstrap == null) return;

            if (_console == null && _bootstrap.IsEditor && _bootstrap.DebugOnEdtor)
            {
                Debug.Log(debug);
                return;
            }

            if (_console == null) return;

            _console.Print(debug);
        }

        protected virtual void CallReset()
        {
            if (Reset != null) Reset();
        }

        protected void CallResetDependencies()
        {
            if (ResetDependencies != null) ResetDependencies();
        }

        public virtual void OnObstacleColision()
        {
            if (ObstacleCollision != null) ObstacleCollision();
        }
    }
}