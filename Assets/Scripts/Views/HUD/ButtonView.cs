using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Button))]
    public class ButtonView : GenericView
    {
        protected Button _bt;

        public bool PlaySound = true;
        public bool ResetTimeout = true;

        protected override void OnAwake()
        {
            _bt = GetComponent<Button>();
            _bt.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            if (PlaySound && _sfxController != null)
                _sfxController.PlaySound("Click");

            if(_timeController!=null)
                _timeController.TimeoutReset();
        }
    }
}