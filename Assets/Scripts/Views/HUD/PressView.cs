using InterativaSystem.Controllers;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(PressGesture))]
    [RequireComponent(typeof(ReleaseGesture))]
    public class PressView : GenericView
    {
        protected PressGesture _pressGesture;
        protected ReleaseGesture _releaseGesture;
        protected BoxCollider _boxCollider;
        protected InputController inputController;

        public bool UseIndex;
        public int Index;

        protected override void OnStart()
        {
            base.OnStart();
            inputController = _controller as InputController;
            _pressGesture = GetComponent<PressGesture>();
            _boxCollider = GetComponent<BoxCollider>();
            _releaseGesture = GetComponent<ReleaseGesture>();

            if (UseIndex)
            {
                inputController.AddPressGesture(_pressGesture, Index);
                inputController.AddReleaseGesture(_releaseGesture, Index);
            }
            else
            {
                inputController.AddPressGesture(_pressGesture);
                inputController.AddReleaseGesture(_releaseGesture);
            }
        }
    }
}