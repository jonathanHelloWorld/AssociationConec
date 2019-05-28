using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class NetworkConquerShowRank : ButtonView
    {
        private NetworkServerController NetController;

        public bool Show;

        protected override void OnStart()
        {
            base.OnStart();

            NetController = _bootstrap.GetController(_controllerType) as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

#if HAS_SERVER
            if(Show)
                NetController.SendMessageToAll("NetworkShowRank", "");
            else
                NetController.SendMessageToAll("NetworkHideRank", "");
#endif
        }
    }
}