using InterativaSystem.Interfaces;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Time Controller")]
    public class TimeController : GenericController
    {
        public float TimeSpeed = 1;


		[HideInInspector]
        public float TimeSinceAppStart, TimeSinceGameStart, RemainingAppTime, RemainingGameTime, TimeSinceLastTimeout;
        [HideInInspector]
        public bool AppTimeup, GameTimeup, Timeup;
        public bool Paused = false;

        public event SimpleEvent AppTimeout , GameTimeout, Timeout;
		public event RegisterEvent RegisterInJson;
        public event StringEvent OnUpdateRemainingGameTime, OnUpdateRemainingAppTime, OnUpdateGameTime, OnUpdateAppTime;

        public float AppTimeLimit = 90f, GameTimeLimit = 30f, TimeoutLimit = 120f;
        [Space]
        public float RemainingTimeoutTime;

        private IController _gameController;
        
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Time;
        }

		void Update()
        {
            if (Paused)
                return;

            if (_bootstrap.IsAppRunning && !AppTimeup)
            {
                TimeSinceAppStart += Time.deltaTime;
                RemainingAppTime = AppTimeLimit - TimeSinceAppStart;

                if (OnUpdateAppTime != null)
                    OnUpdateAppTime(TimeSinceAppStart.ToString());

                if (OnUpdateRemainingAppTime != null)
                    OnUpdateRemainingAppTime(RemainingAppTime.ToString());

                if (RemainingAppTime <= 0)
                {
                    if (AppTimeout != null) AppTimeout();
                    AppTimeup = true;
                }
            }

            if (!GameTimeup && _gameController != null && _gameController.IsGameRunning)
            {
				//Debug.Log("gamee");
                TimeSinceGameStart += Time.deltaTime;
                RemainingGameTime = GameTimeLimit - TimeSinceGameStart;

                if (OnUpdateGameTime != null)
                    OnUpdateGameTime(TimeSinceGameStart.ToString());

                if (OnUpdateRemainingGameTime != null)
                    OnUpdateRemainingGameTime(RemainingGameTime.ToString());

				if(ControllerNewAssociation.Instance.checkQtd == ControllerNewAssociation.Instance.qtdContainers) 
				{
					ControllerNewAssociation.Instance.ended = true;

					if (RegisterInJson != null) 
					{
						float result = 60.0f - RemainingGameTime;
						RegisterInJson("timeInGame", result.ToString());
						RegisterInJson("FaseConcluida", "true");
					}

					if (Timeout != null) Timeout();
					Timeup = true;

					CallAction(0);
					ControllerNewAssociation.Instance.checkQtd = 0;
					ControllerNewAssociation.Instance.OnGameStartReset();
				}



                if (RemainingGameTime <= 0  && !ControllerNewAssociation.Instance.ended)
                {
					
					if (RegisterInJson != null && ControllerNewAssociation.Instance.checkQtd < ControllerNewAssociation.Instance.qtdContainers) 
					{
						UnityEngine.Debug.Log("foiEcvent");
						RegisterInJson("FaseConcluida", "false");
					}
					ControllerNewAssociation.Instance.OnGameStartReset();
					ControllerNewAssociation.Instance.checkQtd = 0;
					CallAction(1);

					if (Timeout != null &&  ControllerNewAssociation.Instance.checkQtd < ControllerNewAssociation.Instance.qtdContainers ) Timeout();
					Timeup = true;

					if (GameTimeout != null) GameTimeout();
                    GameTimeup = true;
                }
            }

            if (!Timeup)
            {
                TimeSinceLastTimeout += Time.deltaTime;
                RemainingTimeoutTime = TimeoutLimit - TimeSinceLastTimeout;
				
                if (RemainingTimeoutTime <= 0)
                {
                   /* if (Timeout != null) Timeout();
                    Timeup = true;
					*/
                }
            }

            Time.timeScale = TimeSpeed;
        }

        protected override void OnStart()
        {
            _bootstrap.AppStarted += StartAppTimer;
            _bootstrap.GameControllerStarted += SetGameController;
        }

        void SetGameController()
        {
            StartGameTimer();
            _gameController = _bootstrap.ActualRunningGameController;
            _gameController.OnGameStart += StartGameTimer;
            _gameController.OnGameEnd += UnsetGameController;

            if (_gameController.GameTime > 0)
                GameTimeLimit = _gameController.GameTime;
        }
        void UnsetGameController()
        {
            _gameController = null;
        }
        
        public void TimeoutReset()
        {
            TimeSinceLastTimeout = 0;
            Timeup = false;
        }
        public void StartAppTimer()
        {
            TimeSinceAppStart = 0;
            AppTimeup = false;
        }
        public void StartGameTimer()
        {
            DebugLog("StartGameTimer");
            TimeSinceGameStart = 0;
            GameTimeup = false;
            Paused = false;
        }

        public void ResetTimeScale()
        {
            TimeSpeed = 1;
        }
        public void AddToTimeScale(float value)
        {
            TimeSpeed += value;
        }
        public void SetToTimeScale(float value)
        {
            TimeSpeed = value;
        }
    }
}