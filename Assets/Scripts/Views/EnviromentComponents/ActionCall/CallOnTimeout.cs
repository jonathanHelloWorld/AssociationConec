namespace InterativaSystem.Views.EnviromentComponents.ActionCall
{
    public class CallOnTimeout : GenericView
    {
        public int Id;

        protected override void OnStart()
        {
            base.OnStart();

            _timeController.Timeout += CallId;
        }

        void CallId()
        {
            _controller.CallAction(Id);
        }
    }
}