using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.IO
{
    public class IOInputAbsoluteDirectory : InputView
    {
        private IOController _io;
        protected override void OnStart()
        {
            base.OnStart();

            _io = _controller as IOController;
            input.text = _io.DataFolder;
        }

        protected override void ValueChanged(string value)
        {
            base.ValueChanged(value);
            _io.DataFolder = value;
        }

        protected override void EndEdit(string value)
        {
            base.EndEdit(value);
            _io.DataFolder = value;
            _io.SetUpFolder();
        }
    }
}