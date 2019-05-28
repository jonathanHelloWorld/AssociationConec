using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssocSendFeedbackButton : ButtonView
    {
        private AssociationController _associationController;

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            _bt.interactable = _associationController.StaticAssociated >= _associationController.AssociationsCount;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _associationController.SendFeedback();
        }
    }
}