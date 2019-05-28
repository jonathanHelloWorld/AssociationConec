using System;
using System.Collections.Generic;
using InterativaSystem.Controllers.Run;
using InterativaSystem.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Score Controller")]
    public class ScoreController : GenericController
    {
        protected ScoreData _scoreData;

        public event ScoreData.ListScoreEvent OnUpdateScoreboard;
        public event ScoreData.ScoreEvent OnUpdateScore;
        public event SimpleEvent OnValueUpdate;

        #region Initialization
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Score;
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void GetReferences()
        {
            base.GetReferences();

            _scoreData = _bootstrap.GetModel(ModelTypes.Score) as ScoreData;

            _scoreData.OnScoreDataUpdated += UpdateCall;
            _scoreData.OnNewValue += OnValueUpdateEvent;
            _scoreData.OnDataUpdated += OnValueUpdateEvent;

            _bootstrap.AppStarted += RequestScoreboard;
            _bootstrap.GameControllerEnded += RequestScoreboard;
        }

        void OnValueUpdateEvent()
        {
            if (OnValueUpdate != null) OnValueUpdate();
        }

        #endregion

        #region GetSetters
        public void RequestScoreboard()
        {
            if (OnUpdateScoreboard != null) OnUpdateScoreboard(_scoreData.GetScoreboard());
        }

        public ScoreValue GetScore(string uid)
        {
            return _scoreData.GetScore(uid);
        }
        public bool TryGetScore(string uid, out ScoreValue value)
        {
            value = null;
            if (_scoreData.TryGetScore(uid, out value))
            {
                return true;
            }
            return false;
        }
        //Setters
        public void AddScore(int value)
        {
            _scoreData.AddToActualValue(value);
        }

        internal void AddScore(int value, float time)
        {
            _scoreData.AddToActualValue(value, time);
        }

        public void AddScore(int value, float time, int type)
        {
            _scoreData.AddToActualValue(value, time, type);
        }
        #endregion

        private void UpdateCall(ScoreValue value)
        {
            if (OnUpdateScore != null) OnUpdateScore(value);
        }

        protected override void CallReset()
        {
            base.CallReset();
        }
    }
}