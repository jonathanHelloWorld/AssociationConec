using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.Ids
{
    public class IdNewSessionButton : ButtonView
    {
        private IdsController _idsController;
        protected override void OnStart()
        {
            base.OnStart();

            _idsController = _controller as IdsController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _idsController.CreateNewSession();
        }
    }
}