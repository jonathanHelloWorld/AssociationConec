using InterativaSystem.Controllers;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.Gestures
{
    [RequireComponent(typeof(PanGesture))]
    public class InputAddPanGesture : GenericView
    {
        private PanGesture _pg;

        protected override void OnStart()
        {
            base.OnStart();

            _pg = GetComponent<PanGesture>();

            var input = _controller as InputController;

            input.AddPanGesture(_pg);
        }
    }
}