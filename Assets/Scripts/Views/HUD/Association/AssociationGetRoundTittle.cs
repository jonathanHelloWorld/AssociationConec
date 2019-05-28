using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssociationGetRoundTittle : TextView
    {
        private AssociationController _associationController;
        public bool IsTittleB;

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;

            _associationController.OnRoundChange += ChangeText;
        }

        private void ChangeText(int value)
        {
            _tx.text = _associationController.GetRoundTittle(IsTittleB);
        }
    }
}