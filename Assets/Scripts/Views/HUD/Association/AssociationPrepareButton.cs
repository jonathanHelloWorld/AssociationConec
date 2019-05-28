using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssociationPrepareButton : ButtonView
    {
        public bool HardIndex;
        public int QuestionIndex;

        private AssociationController _associationController;

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;
        }


        protected override void OnClick()
        {
            base.OnClick();

            if (HardIndex)
                _associationController.PrepareGame(QuestionIndex);
            else
                _associationController.PrepareGame();
        }
    }
}