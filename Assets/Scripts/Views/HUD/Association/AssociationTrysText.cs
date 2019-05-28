using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssociationTrysText : DynamicText
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

            UpdateText(_associationController.GetTrys().ToString(format));
        }
    }
}