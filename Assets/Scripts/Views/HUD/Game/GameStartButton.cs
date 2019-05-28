using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Run
{
    public class GameStartButton : ButtonView
    {
        protected override void OnClick()
        {
            base.OnClick();
            _bootstrap.StartGame(_controller);
        }
    }
}