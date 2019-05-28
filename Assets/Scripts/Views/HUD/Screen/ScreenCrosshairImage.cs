using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Screen
{
    public class ScreenCrosshairImage : ImageView
    {
        private ScreenInfo _screen;
        protected override void OnStart()
        {
            base.OnStart();
            _screen = _bootstrap.GetModel(ModelTypes.Screen) as ScreenInfo;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            transform.position = _screen.EndPoint;
            transform.LookAt(_screen.transform.position);
        }
    }
}