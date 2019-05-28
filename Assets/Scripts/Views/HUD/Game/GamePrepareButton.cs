using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Game
{
    public class GamePrepareButton : ButtonView
    {
        protected override void OnClick()
        {
            base.OnClick();
            _controller.PrepareGame();
        }
    }


}