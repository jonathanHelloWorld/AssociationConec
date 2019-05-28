using InterativaSystem.Controllers;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(FlickGesture))]
    public class FlickView : GenericView
    {
        protected FlickGesture _flickGesture;
        protected BoxCollider _boxCollider;
        protected InputController inputController;

        protected override void OnStart()
        {
            base.OnStart();

            _flickGesture = GetComponent<FlickGesture>();
            _boxCollider = GetComponent<BoxCollider>();

            inputController = _controller as InputController;

            inputController.AddFlickGesture(_flickGesture);
        }
    }
}