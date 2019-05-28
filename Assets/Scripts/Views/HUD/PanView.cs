using InterativaSystem.Controllers;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(PanGesture))]
    public class PanView : GenericView
    {
        protected PanGesture _panGesture;
        protected BoxCollider _boxCollider;
        protected InputController inputController;

        protected override void OnStart()
        {
            base.OnStart();

            _panGesture = GetComponent<PanGesture>();
            _boxCollider = GetComponent<BoxCollider>();

            inputController = _controller as InputController;

            inputController.AddPanGesture(_panGesture);
        }
    }
}