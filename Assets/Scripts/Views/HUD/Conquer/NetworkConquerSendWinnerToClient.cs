using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class NetworkConquerSendWinnerToClient : ButtonView
    {
        private ConquerController conquerController;

        public int Id;
        public bool GetLive;

        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if(GetLive)
                Id = conquerController.winner;
        }

        protected override void OnClick()
        {
            base.OnClick();

#if HAS_SERVER
            conquerController.NetworkSendWinner(Id);
#endif
        }
    }
}