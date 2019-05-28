namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallButtonActionEnded : ButtonView
    {
        public int Id;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnClick()
        {
            base.OnClick();

            _controller.ActionEnded(Id);
        }
    }
}