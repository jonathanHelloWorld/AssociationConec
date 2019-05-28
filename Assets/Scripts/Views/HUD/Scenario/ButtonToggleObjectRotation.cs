using InterativaSystem.Views.EnviromentComponents;

namespace InterativaSystem.Views.HUD
{
    public class ButtonToggleObjectRotation : ButtonView
    {
        public GameObjectAutoRotate Target;

        public bool ForceState;
        public bool IsOn;

        protected override void OnClick()
        {
            base.OnClick();

            if (ForceState)
                Target.CanRotate = IsOn;
            else
                Target.CanRotate = !Target.CanRotate;
        }
    }
}