using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Time
{
    public class TimeGameText : DynamicText
    {
        public ConvertTimeType TimeFormat = ConvertTimeType.ms;

        protected override void OnStart()
        {
            _timeController = _controller as TimeController;
            _timeController.OnUpdateGameTime += UpdateData;
        }

        protected override void UpdateData(string value)
        {
            string nText = Utils.ConvertRealTime(float.Parse(value), TimeFormat);
            UpdateText(nText);
        }
    }
}