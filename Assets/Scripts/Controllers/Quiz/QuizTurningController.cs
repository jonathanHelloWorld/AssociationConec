using System.Collections;
using System.Collections.Generic;
using InterativaSystem.Models;
using InterativaSystem.Services;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Quiz Turning Controller")]
    public class QuizTurningController : QuizController
    {
        private UDPSender sender;
#if HAS_SERVER
        private TurningVoteData turningVoteData;
#endif

        public event SimpleEvent ClodeTurning;

        public bool UseUDP;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Quiz;

            QuestionsRight = 0;
            _questionCount = 0;
        }

        protected override void OnStart()
        {
            base.OnStart();

            if(UseUDP)
                sender = _bootstrap.GetService(ServicesTypes.UDPSend) as UDPSender;
        
#if HAS_SERVER
            turningVoteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
#endif
        }

        public override void StartGame()
        {
            base.StartGame();
            if (UseUDP)
                OpenTurningVote();
        }

        protected override void TimeOut()
        {
#if HAS_SERVER
            if (!_isServer)
                _clientController.SendMessageToServer("NetworkCloseUDPVote", "");
#endif
            base.TimeOut();
        }

        public override void EndGame()
        {
            base.EndGame();

            if (UseUDP)
            {
                CloseTurningVote();
            }
        }
        
        public void OpenTurningVote()
        {
            sender.SendPacket("OpenVote");
        }
        public void CloseTurningVote()
        {
            sender.SendPacket("CloseVote");

            if (ClodeTurning != null) ClodeTurning();
        }

        public override void OpenUDPVote()
        {
            base.OpenUDPVote();
            OpenTurningVote();
        }
        public override void CloseUDPVote()
        {
            base.CloseUDPVote();
            CloseTurningVote();

#if HAS_SERVER
            turningVoteData.NetworkSendVotes();
#endif
        }

        public bool CheckIfVoteIsRight(string alt)
        {
            int vote = int.Parse(alt);
            return _questions.CheckAnswer(vote);
        }

        public int AlternativesCount()
        {
            return _questions.Questions[_questions.ActualQuestion].alternatives.Count;
        }


#if HAS_SERVER
        public override void NetworkCloseUDPVote(string json)
        {
            base.NetworkCloseUDPVote(json);
            
            CloseUDPVote();
        }
#endif
    }


}