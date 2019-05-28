using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Register
{
    public class RegisterButtonOption : ButtonView
    {
        protected RegisterController _registerController;
        public string DataName;
        public string value;

        public bool IsUnique;

        protected override void OnStart()
        {
            base.OnStart();

            _registerController = _controller as RegisterController;
            _registerController.OnSubmit += Submit;

#if HAS_SERVER
            _registerController.OnRegistryReceiveEnded += Submit;
#endif

            _registerController.Reset += Reset;


            if (IsUnique)
            {
                _bt.interactable = Check();
            }
        }
        

        protected override void OnClick()
        {
            base.OnClick();

            _registerController.AddRegisterValue(DataName, value, false);
            //Todo Yuri: Reover gambirra feita no projeto do quiz
            _registerController.FieldsBools[0] = true;
        }

        private bool Check()
        {
            if (IsUnique)
            {
                string reg;
                if (_registerController.TryGetRegistryFromValue(DataName, value, out reg))
                {
                    return reg != null && reg != value;
                }
            }
            return true;
        }


        private void Reset()
        {
            if (IsUnique)
            {
                _bt.interactable = Check();
            }
        }

        private void Submit()
        {
            if (IsUnique)
            {
                _bt.interactable = Check();
            }
        }
    }
}