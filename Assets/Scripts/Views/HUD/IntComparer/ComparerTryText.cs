using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.IntComparer
{
    public class ComparerTryText : TextView
    {
        private IntComparerController _intComparer;
        private bool Remaining = true;
        protected override void OnStart()
        {
            base.OnStart();
            _intComparer = _controller as IntComparerController;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            _tx.text = Remaining ? (-_intComparer.trys + _intComparer.TrysLimit).ToString("00") : _intComparer.trys.ToString("00");
        }
    }
}