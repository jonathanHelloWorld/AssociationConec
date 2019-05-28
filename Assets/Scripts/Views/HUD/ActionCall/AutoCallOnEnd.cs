namespace InterativaSystem.Views.HUD.ActionCall
{
    public class AutoCallOnEnd : GenericView
    {
        public int Id;

        protected override void OnStart()
        {
            base.OnStart();
            _controller.GenericActionEnded += ActionEnded;
        }

        private void ActionEnded(int value)
        {
            if (Id - 1 != value) return;

            _controller.CallAction(Id);
        }
    }
}