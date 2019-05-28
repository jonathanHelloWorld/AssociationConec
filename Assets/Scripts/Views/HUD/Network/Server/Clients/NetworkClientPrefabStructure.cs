using System;
using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Network.Server.Clients
{
    public class NetworkClientPrefabStructure : GenericView
    {
        #region Inspector
        [Header("Settings")]
        public NetworkInstanceType Type;
        public string NameRegistry = "Nome";

        [Header("References")]
        public Text Name;

        public Text Id;
        public Text connId;
        
        [Space]
        public GameObject ActiveRoot;
        public GameObject ListenerRoot;
        public GameObject ControllerRoot;
        public Text Score;

        [Space]
        public Image Background;

        [Space]
        public Toggle Prepared;
        public Toggle OnGame;

        [Space]
        public Toggle GameEnded;

        [Space]
        public GenericView GenericView;
        #endregion

        private string myUid;
        
        private NetworkServerController _serverController;
        private ScoreController _scoreController;
        private RegisterController _registration; 

        protected override void OnStart()
        {
            _serverController = _controller as NetworkServerController;

            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            _registration = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _scoreController.OnUpdateScore += UpdateFields;
            _registration.OnRegistryUpdate += UpdateFields;
            _serverController.OnClientsReset += ResetStats;
            _serverController.OnClientStatsChange += UpdateStats;
            
            ResetData();

            Id.text = myUid;

            UpdateConnectionId();

            switch (Type)
            {
                case NetworkInstanceType.ListeningClient:
                    if (ActiveRoot)
                        ActiveRoot.SetActive(false);
                    if(ListenerRoot)
                        ListenerRoot.SetActive(true);
                    if (ControllerRoot)
                        ControllerRoot.SetActive(false);
                    break;
                case NetworkInstanceType.ActiveClient:
                    if (ActiveRoot)
                        ActiveRoot.SetActive(true);
                    if (ListenerRoot)
                        ListenerRoot.SetActive(false);
                    if (ControllerRoot)
                        ControllerRoot.SetActive(false);
                    break;
                case NetworkInstanceType.ControllerClient:
                    if (ActiveRoot)
                        ActiveRoot.SetActive(false);
                    if (ListenerRoot)
                        ListenerRoot.SetActive(false);
                    if (ControllerRoot)
                        ControllerRoot.SetActive(true);
                    break;
            }

            if(GenericView!=null)
                GenericView.Initialize();
        }

        void ResetData()
        {
            if (Prepared != null)
                Prepared.isOn = false;
            if (OnGame != null)
                OnGame.isOn = false;
            if (GameEnded != null)
                GameEnded.isOn = false;

            Id.text = "";
            connId.text = "00";
            Score.text = "000";
            Name.text = "";

        }

        public void ChangeBackgroundColor(Color color)
        {
            if (Background == null) return;

            Background.color = color;
        }

        void UpdateScore()
        {
            if (Score == null) return;

            var scr = _scoreController.GetScore(myUid);

            UnityEngine.Debug.Log(scr);

            Score.text = scr != null ? scr.value.ToString("000") : "000";
        }
        void UpdateName()
        {
            if (Name == null) return;
            var reg = _registration.GetRegistry(myUid);
            if (reg != null && reg.registry.ContainsKey(NameRegistry))
                Name.text = reg.registry[NameRegistry];
            else
                Name.text = "NotDefined";
        }
        void UpdateStats(NetworkClientObject cl)
        {
            if(cl.uid != myUid) return;

            if (Prepared != null)
                Prepared.isOn = cl.isGamePrepared;
            if (OnGame != null)
                OnGame.isOn = cl.isGameStarted;
            if (GameEnded != null)
                GameEnded.isOn = cl.isGameEnded;
        }
        void ResetStats()
        {
            if (Prepared != null)
                Prepared.isOn = false;
            if (OnGame != null)
                OnGame.isOn = false;
            if (GameEnded != null)
                GameEnded.isOn = false;
        }

        public string GetUid()
        {
            return myUid;
        }
        public void SetUid(string uid)
        {
            myUid = uid;

            ResetData();

            Id.text = myUid;

            UpdateConnectionId();
        }
        
        public void UpdateFields()
        {
            UpdateScore();
            UpdateName();
            UpdateConnectionId();
        }

        private void UpdateConnectionId()
        {
            if (_serverController == null) return;
            var conn = _serverController.GetConnection(myUid);
            if (conn != null)
                connId.text = _serverController.GetConnection(myUid).connectionId.ToString();
            else
                connId.text = "--";
        }

        private void UpdateFields(ScoreValue value)
        {
            UpdateFields();
            UpdateConnectionId();
        }
    }
}