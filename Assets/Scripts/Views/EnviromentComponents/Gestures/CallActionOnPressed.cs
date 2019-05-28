using System;

namespace InterativaSystem.Views.EnviromentComponents.Gestures
{
    public class CallActionOnPressed : GesturePressView
    {
        public int Id;

        protected override void OnPressed(object sender, EventArgs e)
        {
            base.OnPressed(sender, e);

            _controller.CallAction(Id);
        } 
    }
}