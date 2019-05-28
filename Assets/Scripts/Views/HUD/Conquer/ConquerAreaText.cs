using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class ConquerAreaText : TextView
    {
        private ConquerController conquerController;
        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            _tx.text = conquerController.AreaToConquer.ToString();
        }
    }
}