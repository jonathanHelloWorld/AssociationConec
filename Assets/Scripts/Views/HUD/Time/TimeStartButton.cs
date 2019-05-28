using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Time
{
    public class TimeStartButton : ButtonView
    {
        private TimeController _timer;

        public bool IsApp;
        protected override void OnStart()
        {
            base.OnStart();

            _timer = _controller as TimeController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if(IsApp)
                _timer.StartAppTimer();
            else
                _timer.StartGameTimer();
        }
    }
}