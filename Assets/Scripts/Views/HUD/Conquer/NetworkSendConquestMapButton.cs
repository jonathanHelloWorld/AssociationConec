using InterativaSystem.Controllers;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class NetworkSendConquestMapButton : ButtonView
    {
        private ConquerController conquerController;

        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

#if HAS_SERVER
            conquerController.CallConquestMap();
#endif
        }
    }
}
