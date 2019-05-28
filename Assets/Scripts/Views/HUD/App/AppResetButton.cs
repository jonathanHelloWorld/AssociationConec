using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Game
{
    public class AppResetButton : ButtonView
    {
        protected override void OnClick()
        {
            base.OnClick();
            _bootstrap.ResetApp();
        }
    }
}