using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;

namespace Assets.Scripts.Views.HUD.Console
{
    public class ConsoleLineText : TextView
    {
        private ConsoleController _console;
        protected override void OnStart()
        {
            _console = _controller as ConsoleController;
            _console.ConsolePrint += Write;
        }

        private void Write(string value)
        {
            _tx.text = value;
        }
    }
}