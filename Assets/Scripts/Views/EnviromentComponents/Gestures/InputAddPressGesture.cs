using InterativaSystem.Controllers;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.Gestures
{
    public class InputAddPressGesture : GesturePressView
    {
        protected override void OnStart()
        {
            base.OnStart();

            var input = _controller as InputController;
            input.AddPressGesture(_pg);
        }
    }
}