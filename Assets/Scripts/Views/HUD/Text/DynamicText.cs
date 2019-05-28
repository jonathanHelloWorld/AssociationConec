using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using InterativaSystem.Controllers;
using UnityEngine.UI;
using InterativaSystem.Interfaces;
using InterativaSystem.Views.HUD;
using InterativaSystem.Models;

namespace Assets.Scripts.Views.HUD
{
    public class DynamicText : TextView
    {
        public string preText = "";
        public string afterText = "";

        public string format = "00000";

        protected override void OnStart()
        {
            base.OnStart();

            _tx = GetComponent<Text>();
        }

        protected void UpdateText(string text)
        {
            string newText = preText + text + afterText;

            _tx.text = newText;
        }

        protected virtual void UpdateData(string value) { }
        protected virtual void UpdateData(float value) { }
        protected virtual void UpdateData(bool value) { }
        protected virtual void UpdateData(ScoreValue value) { }
    }
}