using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Game
{
    public class CanvasOnGameEnd : CanvasGroupView
    {
        public bool TurnOff;
        protected override void OnStart()
        {
            base.OnStart();

            if(TurnOff)
                _controller.OnGameEnd += Hide;
            else
                _controller.OnGameEnd += Show;
        }
    }
}