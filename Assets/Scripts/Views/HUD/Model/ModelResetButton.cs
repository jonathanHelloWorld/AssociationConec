using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Model
{
    public class ModelResetButton : ButtonView
    {
        public ModelTypes ModelType;
        private GenericModel model;

        protected override void OnStart()
        {
            base.OnStart();

            model = _bootstrap.GetModel(ModelType);
        }

        protected override void OnClick()
        {
            base.OnClick();

            model.CallReset();
        }
    }
}