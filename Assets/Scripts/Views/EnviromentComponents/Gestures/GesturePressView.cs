using System;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.Gestures
{
    [RequireComponent(typeof(PressGesture))]
    public class GesturePressView : GenericView
    {
        protected PressGesture _pg;
        protected override void OnStart()
        {
            base.OnStart();

            _pg = GetComponent<PressGesture>();

            _pg.Pressed += OnPressed;
        }

        protected virtual void OnPressed(object sender, EventArgs e) { }
    }
}