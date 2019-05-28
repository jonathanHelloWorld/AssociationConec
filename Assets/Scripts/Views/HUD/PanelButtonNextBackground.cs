namespace InterativaSystem.Views.HUD
{
    public class PanelButtonNextBackground : ButtonView
    {
        public PanelChangeBackground Panel;

        public bool IsNext = true;

        protected override void OnClick()
        {
            base.OnClick();

            Panel.NextBg(IsNext);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!IsNext)
                _bt.interactable = Panel._actualBg != 0;
            else
                _bt.interactable = Panel._actualBg != Panel.Backgrounds.Length-1;
        }
    }
}