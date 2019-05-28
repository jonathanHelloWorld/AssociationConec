using System;
using System.Collections;
using System.Linq;
using System.Threading;
using DG.Tweening;
using InterativaSystem.Models;
using InterativaSystem.Views;
using InterativaSystem.Views.ControllableCharacters;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Controllers.Run
{
    [AddComponentMenu("ModularSystem/Controllers/Run/Run Controller")]
    public class RunController : GenericController
    {
        [Header("Settings")]
        public float PoolingDistance = 5;
        public float Speed = 20f;

        public float RecoveryTime = 0.8f;

        [HideInInspector]
        public bool StartSet;

        [HideInInspector]
        public float FixedSpeed;

        //TODO Yuri: Remover o BlockAll
        public bool BlockAll;
        
        protected ResourcesDataBase _resources;
        protected CharacterSelectionData _characterSelection;

        public float DistanceTraveled = 0;

        [Obsolete]
        private int _selectedChar;
        [Obsolete]
        public int SelectedChar
        {
            get { return this._selectedChar; }
            set
            {
                this._selectedChar = value;
            }
        }

        [HideInInspector]
        public float MaxSpeed;

        protected bool _counted, _gameEnding;

        protected TimeController _timeController;

        [Header("Dynamic Curve")]
        [HideInInspector]
        public float Curve;

        [Range(0, 0.2f)]
        public float CurveMax;

        protected float _curve, _animatedCurve;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Run;

            FixedSpeed = Speed;
            MaxSpeed = FixedSpeed;
        }

        protected override void OnStart()
        {
            if (BlockAll) return;
            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            _characterSelection = _bootstrap.GetModel(ModelTypes.Characters) as CharacterSelectionData;

            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            _timeController.GameTimeout += TimeOut;
        }

        protected void TimeOut()
        {
            if (BlockAll) return;
            _bootstrap.EndGame(this);
        }
        
        protected void Update()
        {
            if (BlockAll) return;
            if (!_bootstrap.IsAppRunning || !IsGameStarted || !_counted)
                return;

            DistanceTraveled += Time.deltaTime * Speed;

            var time = 3;
            if (_timeController.RemainingGameTime < time && !_gameEnding)
            {
                _gameEnding = true;
                DOTween.To(x => Speed = x, FixedSpeed, 0, time);
            }
        }

        //Curve control for special shadders
        IEnumerator ControlCruveFlow()
        {
            var rnd = new System.Random();

            while (!_gameEnding)
            {
                var value = rnd.NextDouble();
                ChangeCurveDirection((float)(value * CurveMax));

                yield return null;
                value = rnd.NextDouble();
                var t = PoolingDistance/Speed;
                yield return new WaitForSeconds((float)(value * t + t/2));
            }
        }

        protected void ChangeCurveDirection(float dir)
        {
            Curve = dir;
            _curve = dir;
        }

        public override void StartGame()
        {
            if (BlockAll) return;
            base.StartGame();

            //StartCoroutine(ControlCruveFlow());

            _gameEnding = false;

            var rnd = new System.Random();
            var value = rnd.NextDouble();
            ChangeCurveDirection((float)(value * CurveMax));

            /*var tr = new Thread(() =>
            {
                Count();
            });
            tr.Start();*/

            StartCoroutine(Count());
        }

        public override void PrepareGame()
        {
            if (BlockAll) return;
            base.PrepareGame();

            Speed = 0f;
            _counted = false;
            DistanceTraveled = 0f;
            InstantiateCharacter();
        }

        protected virtual void InstantiateCharacter()
        {
            var selectedChar = _characterSelection.GetSelectedCharacter();

            //Instantiate the character based on his category and id
            var temp = Instantiate(Resources.Load<GameObject>(_resources.Prefabs.Find(x => x.category == selectedChar.prefabCategory && x.id == selectedChar.id).name));
            var views = temp.GetComponents<GenericView>();

            var chViews = temp.transform.GetComponentsInChildren<GenericView>();

            for (int i = 0, n = views.Length; i < n; i++)
            {
                views[i].Initialize();
            }
            for (int i = 0, n = chViews.Length; i < n; i++)
            {
                chViews[i].Initialize();
            }
        }
        protected virtual void InstantiateCharacter(int id, int track, string clId)
        {
            //Instantiate the character based on his category and id
            var temp = Instantiate(Resources.Load<GameObject>(_resources.Prefabs.Find(x => x.category == PrefabCategory.Character && x.id == id).name));
            var views = temp.GetComponents<GenericView>();

            if (temp.GetComponent<RunCharacterListener>() != null)
            {
                temp.GetComponent<RunCharacterListener>().trackId = track;
                temp.GetComponent<RunCharacterListener>().Id = clId;
            }

            for (int i = 0, n = views.Length; i < n; i++)
            {
                views[i].Initialize();
            }
        }

        protected override void CallReset()
        {
            base.CallReset();
            DistanceTraveled = 0;
        }

        public override void OnObstacleColision()
        {
            base.OnObstacleColision();

            if (!_gameEnding)
                DOTween.To(x => Speed = x, 0, FixedSpeed, RecoveryTime);
        }

        IEnumerator Count()
        {
            //Thread.Sleep(1000*3);
            yield return new WaitForSeconds(3f);

            var time = 3;
            _counted = true;
            DOTween.To(x => Speed = x, 0, FixedSpeed, time);
        }

#if HAS_SERVER
#endif
    }
}