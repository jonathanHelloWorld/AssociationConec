using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers.Sound;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Int Comparer Controller")]
    public class IntComparerController : GenericController
    {
        public int DataLimit;
        public int TrysLimit;
        [HideInInspector]
        public int trys;
        [HideInInspector]
        public List<int> datas;

        private SFXController _sfx;
        protected TimeController _timeController;

        public int WinCall, LoseCall, TimeCall;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.IntComparer;
        }

        protected override void CallReset()
        {
            base.CallReset();
            trys = 0;
            datas = new List<int>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;

            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;
            if (_timeController != null)
                _timeController.GameTimeout += TimeOut;
        }

        private void TimeOut()
        {
            CallAction(TimeCall);
            _sfx.PlaySound("Lose");
            datas = new List<int>();

            EndGame();
        }

        public override void PrepareGame()
        {
            datas = new List<int>();
            trys = 0;

            base.PrepareGame();
        }
        
        public void AddData(int value)
        {
            datas.Add(value);
            DebugLog("Added " + value);

            if (datas.Count >= DataLimit)
            {
                ChackData();
            }
        }

        public void ChackData()
        {
            int dif = datas.GroupBy(x => x).Count();

            trys++;

            if (dif == 1)
            {
                CallAction(WinCall);
                _sfx.PlaySound("Win");
                EndGame();
                return;
            }
            else if (trys < TrysLimit)
            {
                CallAction(LoseCall);
            }

            _sfx.PlaySound("Lose");

            if (trys >= TrysLimit)
                EndGame();
            else
                datas = new List<int>();
        }
    }
}