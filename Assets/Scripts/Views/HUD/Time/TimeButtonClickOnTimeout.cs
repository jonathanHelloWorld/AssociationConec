using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Time
{
    public class TimeButtonClickOnTimeout : ButtonView
    {
        private TimeController _timer;

        public bool IsApp;
        protected override void OnStart()
        {
            base.OnStart();

            _timer = _controller as TimeController;

            if (IsApp)
                _timer.AppTimeout += Click;
            else
                _timer.GameTimeout += Click;
        }

        void Click()
        {
            if(_bt.interactable)
                _bt.onClick.Invoke();
        }
    }
}