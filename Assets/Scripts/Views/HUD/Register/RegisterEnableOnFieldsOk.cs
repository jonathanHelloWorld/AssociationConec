using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Register
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RegisterEnableOnFieldsOk : GenericView
    {
        private CanvasGroup _cv;
        private RegisterController _registerController;

        protected override void OnStart()
        {
            _registerController = _controller as RegisterController;
            _cv = GetComponent<CanvasGroup>();
        }

        protected override void OnUpdate()
        {
            _cv.interactable = !_registerController.FieldsBools.Contains(false);
        }
    }
}