using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Screen
{
    public class ScreenRadialClickFillImage : ImageView
    {
        private ScreenInfo _screen;

        public bool LookToCamera = true;

        protected override void OnStart()
        {
            base.OnStart();
            _screen = _bootstrap.GetModel(ModelTypes.Screen) as ScreenInfo;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            transform.position = _screen.FillPos;
            _image.fillAmount = _screen.ClickTimeLapse / _screen.ClickDelay;

            if (LookToCamera)
                transform.LookAt(_screen.GetCamera().transform.position);
        }
    }
}