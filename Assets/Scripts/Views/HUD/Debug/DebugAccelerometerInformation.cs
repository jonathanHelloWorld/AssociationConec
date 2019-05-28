using InterativaSystem.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Debug
{
    [RequireComponent(typeof(Text))]
    public class DebugAccelerometerInformation : TextView
    {
        private InputController _inputController;

        protected override void OnStart()
        {
            _inputController = _controller as InputController;
            _inputController.OnDeviceTurn += Print;
        }

        void Print(Vector3 value)
        {
            _tx.text = value.ToString();
        }
    }
}