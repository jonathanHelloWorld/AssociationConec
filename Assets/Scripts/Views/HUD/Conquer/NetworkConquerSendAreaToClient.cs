using InterativaSystem.Controllers;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class NetworkConquerSendAreaToClient : ButtonView
    {
        private ConquerController conquerController;

        public InputField input;

        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

#if HAS_SERVER
            if (input == null)
            {
                conquerController.UpdateAreaToConquer();
                conquerController.NetworkSendArea(conquerController.AreaToConquer);
                return;
            }
            int res;
            if(int.TryParse(input.text, out res))
                conquerController.NetworkSendArea(res);
#endif
        }
    }
}