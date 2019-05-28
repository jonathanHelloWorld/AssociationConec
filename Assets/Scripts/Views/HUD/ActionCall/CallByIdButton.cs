namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallByIdButton : ButtonView
    {
        public int Id;

        public bool delayCall;
        public float delay;

        protected override void OnStart()
        {
            base.OnStart();

        }

        protected override void OnClick()
        {
            base.OnClick();

            if (delayCall)
            {
                Invoke("Call", delay);
            }
            else
            {
                Call();
            }
        }

        void Call()
        {
            _controller.CallAction(Id);
        }
    }
}