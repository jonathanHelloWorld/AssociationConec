using UnityEngine;
using System.Collections;
using InterativaSystem.Models;
using InterativaSystem.ControllersMods;
using System.Linq;
using System.Collections.Generic;

namespace InterativaSystem.Controllers
{
    public class VRCharacterController : GenericController
    {
        QuizController _quizController;
        TimeController _timeController;
        ScoreController _scoreController;
        protected RegisterController _registry;
        RegisterController _registerController;
        VRCharacter _character;

        [Range(0,3)]
        public int cpuLevel = 0;
        [Range(0, 3)]
        public int gpuLevel = 0;
        public QuizModSwitchObjectsOnAnswerFeedback switcheds;
        public Transform finalTeleport;
        public Transform startTeleport;
        public bool startTeleported;
        float teleTimer = 0f;

        void Awake()
        {
            Type = ControllerTypes.VRCharacter;

            //Todo Diego: Corrigir referencia
            //OVRPlugin.cpuLevel = cpuLevel;
            //OVRPlugin.gpuLevel = gpuLevel;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _quizController = _bootstrap.GetController(ControllerTypes.Quiz) as QuizController;

            _registry = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;
            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            _timeController.GameTimeout += Timeout;
            //_timeController.OnUpdateAppTime += StartTeleport;

            _scoreController = _bootstrap.GetController(ControllerTypes.Score) as ScoreController;
            _registerController = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;

            _character = _bootstrap.GetModel(ModelTypes.VRCharacter) as VRCharacter;
        }

        public override void StartGame()
        {
            base.StartGame();

            _timeController.GameTimeLimit = 41f;
        }

        void Update()
        {
            if(IsGameRunning && !startTeleported)
            {
                teleTimer += Time.deltaTime;

                if(teleTimer >= 15f)
                {
                    teleTimer = 0f;
                    startTeleported = true;
                    _character.Fade(false, 1f, ToEntrance);
                }
            }
        }

        void Timeout()
        {
            if (!IsGameRunning) return;

            EndGame();

            if (_quizController._questionCount >= 6)
                _character.Fade(false, 1f, ToTele);
            else
                _character.Fade(false, 1f, ResetCharacter);
        }

        void ToEntrance()
        {
            _character.SetPosition(startTeleport.position, Quaternion.identity);
            _character.Fade(true, 1f, _character.EnableNavMesh);
        }

        void ToTele()
        {
            _character.SetPosition(finalTeleport.position, Quaternion.Euler(0f, 180f, 0f));
            _character.Fade(true, 1f, null);
        }

        void ResetCharacter()
        {
            GameObject qp = null;

            List<ObjectSwitch> sws = switcheds.switchs.Values.ToList();

            while (true)
            {
                ObjectSwitch sw = sws[Random.Range(0, sws.Count)];

                if (!sw.switched && sw.teleport != null)
                {
                    qp = sw.teleport;
                    sw.switched = true;
                    break;
                }
            }

            _character.SetPosition(qp.transform.position, qp.transform.rotation);
            _character.Fade(true, 1f, null);
        }

        protected override void CallReset()
        {
            base.CallReset();

            teleTimer = 0f;
            startTeleported = false;
        }

        public override void EndGame()
        {
            base.EndGame();
        }
    }

    [System.Serializable]
    public class QuestionPosition
    {
        public Transform position;
        public bool used = false;
    }
}