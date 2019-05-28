namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallImageOnById : ImageView
    {
        public int Id;

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += Enable;

            Enable(0);
        }

        private void Enable(int value)
        {
            _image.enabled = Id == value;
        }
    }
}