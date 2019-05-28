using InterativaSystem.Controllers.Sound;

namespace InterativaSystem.Views.HUD.Sound
{
    public class SoundEnableBGMToggle : ToggleView
    {
        private BGMController _bgmController;

        protected override void OnStart()
        {
            base.OnStart();

            _bgmController = _controller as BGMController;
        }

        protected override void Toggled(bool arg0)
        {
            base.Toggled(arg0);

            if(arg0)
                _bgmController.PlaySound();
            else
                _bgmController.StopSound();
        }
    }
}