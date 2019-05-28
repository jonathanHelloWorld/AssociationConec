using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Interfaces;
using InterativaSystem.Models;
using InterativaSystem.Services;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

namespace InterativaSystem
{
    public enum ControllerTypes
    {
        Null = 0,
        Input = 1,
        Score = 2,
        Run = 10,
        CoinPulling = 11,
        ObstaclePulling = 12,
        Register,
        IO,
        Page,
        Time,
        NetworkClient,
        NetworkServer,
        Console,
        ScenarioPulling,
        CurveShader,
        Ranking,
        SoundBGM,
        SoundSFX,
        Association,
        Quiz,
        WebCam,
        Conquer,
        DynamicQuiz,
        MemoryGame,
        CandyGame,
        IntComparer,
        Id,
        Characters,
        Mosaic,
        VRCharacter
    }

    public enum ModelTypes
    {
        Null = 0,
        Screen = 1,
        Resources = 2,
        Tracks = 11,
        Register,
        Score,
        Scoreboard,
        GameSettings,
        Sound,
        Questions,
        TurningVote,
        Grid,
        Group,
        DynamicQuestions,
        MemoryData,
        Ids,
        Network,
        Characters,
        Images,
        Mosaic,
        VRCharacter,
        Association
    }

    public enum ServicesTypes
    {
        Null = 0,
        UDPSend = 1,
        UDPRead = 2,
        SQLite3 = 3,
        WebService,
        SocketIO
    }
}

namespace InterativaSystem
{
    public class Bootstrap : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool LoadScenesOnEditor;
#endif
        public bool DebugOnEdtor;

        public bool LoadScenesOnStartup = false;
        public string[] ScenesToAddOnStart; 

        [HideInInspector]
        public bool IsMobile, IsEditor, IsAppRunning;

        public event GenericController.SimpleEvent OnSceneLoaded, InitializeControllers, InitializeModels, InitializeViews, InitializeServices, AppPaused, AppResumed, AppStarted, AppEnded, GameControllerStarted, GameControllerEnded, Reset;

        [HideInInspector]
        public IController ActualRunningGameController;

        //Singleton
        public static Bootstrap Instance;

        private List<IController> _controllers;
        private List<GenericModel> _models;
        private List<GenericService> _services;

        #region Initialization
        void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(this.gameObject);

            #if UNITY_EDITOR
                Debug.Log("UNITY_EDITOR");
                IsMobile = false;
                IsEditor = true;
            #elif UNITY_STANDALONE
                Debug.Log("UNITY_STANDALONE");
                IsMobile = false;
                IsEditor = false;
            #elif (UNITY_IOS || UNITY_ANDROID)
                Debug.Log("UNITY_MOBILE");
                IsMobile = true;
                IsEditor = false;
            #endif

            _controllers = new List<IController>();
            _models = new List<GenericModel>();
            _services = new List<GenericService>();

            CreateSingleton();
            AddDependeciesScenes();
        }
        void CreateSingleton()
        {
            if (Bootstrap.Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }
        void AddDependeciesScenes()
        {

#if UNITY_EDITOR
            if (!LoadScenesOnEditor)
            {
                Debug.Log("LoadScenesOnStartup " + LoadScenesOnStartup);
                StartCoroutine(EndInitialization());
                return;
            }
#endif

            Debug.Log("LoadScenesOnStartup " + LoadScenesOnStartup);
            if (LoadScenesOnStartup)
            {
                StartCoroutine(LoadScenesOnStartup_Routine());
                return;
            }

            StartCoroutine(EndInitialization());
        }
        public void AddScene(int sceneId)
        {
            StartCoroutine(AddScene_Routine(sceneId));
        }
        IEnumerator AddScene_Routine(int sceneId)
        {
#if UNITY_5_3_OR_NEWER
            yield return SceneManager.LoadSceneAsync(ScenesToAddOnStart[sceneId]);
#elif UNITY_5_2
            yield return Application.LoadLevelAdditiveAsync(ScenesToAddOnStart[sceneId]);
#endif
            if (OnSceneLoaded != null) OnSceneLoaded();

            yield return null;
            StartCoroutine(EndInitialization());
        }
        IEnumerator LoadScenesOnStartup_Routine()
        {
            for (int i = 0, n = ScenesToAddOnStart.Length; i < n; i++)
            {
#if UNITY_5_3_OR_NEWER
                yield return SceneManager.LoadSceneAsync(ScenesToAddOnStart[i]);
#elif UNITY_5_2
                yield return Application.LoadLevelAdditiveAsync(ScenesToAddOnStart[i]);
#endif
                if (OnSceneLoaded != null) OnSceneLoaded();
                yield return null;
            }

            yield return null;
            StartCoroutine(EndInitialization());
        }
        #endregion
        
        #region Get-Set Methods
        public IController GetController(ControllerTypes type)
        {
            if (_controllers.Exists(x => x.Type == type))
            {
                return _controllers.Find(x => x.Type == type);
            }
            else
            {
                //Debug.LogWarning("Controller not founded '" + type + "' returning null");
                return null;
            }
        }
        public void SetController(GenericController controller)
        {
            if (!_controllers.Exists(x => x.Type == controller.Type))
            {
                _controllers.Add(controller);
            }
            else
            {
                Debug.LogError("Controller type already exists ("+ controller.Type + ")");
            }
        }
        public GenericModel GetModel(ModelTypes type)
        {
            if (_models.Exists(x => x.Type == type))
            {
                return _models.Find(x => x.Type == type);
            }
            else
            {
                Debug.LogError("Model not founded " + type);
                return null;
            }
        }
        public void SetModel(GenericModel model)
        {
            if (!_models.Exists(x => x.Type == model.Type))
            {
                _models.Add(model);
            }
            else
            {
                Debug.LogError("Model type already exists " + model);
            }
        }
        public GenericService GetService(ServicesTypes type)
        {
            if (_services.Exists(x => x.Type == type))
            {
                return _services.Find(x => x.Type == type);
            }
            else
            {
                Debug.LogWarning("Controller not founded '" + type + "' returning null");
                return null;
            }
        }
        public void SetService(GenericService service)
        {
            if (!_services.Exists(x => x.Type == service.Type))
            {
                _services.Add(service);
            }
            else
            {
                Debug.LogError("Controller type already exists");
            }
        }
        #endregion

        #region App Manipulation Methods
        public void PauseApp()
        {
            if (!IsAppRunning)
                return;

            IsAppRunning = false;
            if (AppPaused != null) AppPaused();
        }
        public void ResumeApp()
        {
            if (IsAppRunning)
                return;

            IsAppRunning = true;
            if (AppResumed != null) AppResumed();
        }
        public void StartApp()
        {
            Debug.Log("App Starting");
            IsAppRunning = true;
            if (AppStarted != null) AppStarted();
        }
        public void EndApp()
        {
            IsAppRunning = false;
            if (AppEnded != null) AppEnded();
        }
        public void ResetApp()
        {
            Debug.Log("App Reseting");
            EndApp();

            if (Reset != null) Reset();

            StartApp();
        }
        #endregion
        
        #region Network Methods
#if HAS_SERVER
        protected NetworkClientController _clientController;
        public void BootstrapStartGame(string json)
        {
            var gameController = GetController(JsonConvert.DeserializeObject<ControllerTypes>(json));
            StartGame(gameController);

            //_clientController.SendMessageToServer("NetworkGameStarted " + gameController, "");
        }
        public void BootstrapEndGame(string json)
        {
            var gameController = GetController(JsonConvert.DeserializeObject<ControllerTypes>(json));
            EndGame(gameController);

            //_clientController.SendMessageToServer("NetworkGameEnd " + gameController, "");
        }
        public void BootstrapPauseGame(string json)
        {
            var gameController = GetController(JsonConvert.DeserializeObject<ControllerTypes>(json));
            PauseGame(gameController);
        }
        public void BootstrapResumeGame(string json)
        {
            var gameController = GetController(JsonConvert.DeserializeObject<ControllerTypes>(json));
            ResumeGame(gameController);
        }
        protected void CheckNetworCall(string method, string json)
        {
            var _type = this.GetType();
            var meth = _type.GetMethods().ToList();

            if (!meth.Exists(x => x.Name == method)) return;

            var param = new object[] { json };
            _type.GetMethod(method).Invoke(this, param);
        }
#endif
        #endregion

        #region Game Manipulation Methods
        public void StartGame(IController gameController)
        {
            Debug.Log("Game Started " + gameController);
            ActualRunningGameController = gameController;

            if (GameControllerStarted != null) GameControllerStarted();
            ActualRunningGameController.StartGame();
        }
        public void PauseGame(IController gameController)
        {
            if (ActualRunningGameController == null) return;

            ActualRunningGameController.PauseGame();
        }
        public void ResumeGame(IController gameController)
        {
            if (ActualRunningGameController == null) return;

            ActualRunningGameController.ResumeGame();
        }
        public void EndGame(IController gameController)
        {
            Debug.Log("Game Ended " + gameController);
            if (ActualRunningGameController == null) return;

            if (GameControllerEnded != null) GameControllerEnded();
            ActualRunningGameController.EndGame();
        }
        #endregion

        IEnumerator EndInitialization()
        {
            Debug.Log("EndInitialization");
            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }

            if (InitializeControllers != null) InitializeControllers();

            yield return null;

            if (InitializeServices != null) InitializeServices();

            yield return null;

            if (InitializeModels != null) InitializeModels();

            yield return null;

            if (InitializeViews != null) InitializeViews();

            yield return null;

            #if HAS_SERVER
            try
            {
                _clientController = _controllers.Find(x => x.Type == ControllerTypes.NetworkClient) as NetworkClientController;
                _clientController.OnReceive += CheckNetworCall;
            }
            catch (Exception e)
            {
                Debug.LogWarning("this is a server no need for client.\n\n" + e);
            }
            #endif

            yield return null;
            StartApp();
        }
    }
}