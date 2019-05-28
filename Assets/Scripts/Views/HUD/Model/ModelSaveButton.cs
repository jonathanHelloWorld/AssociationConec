using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Model
{
    public class ModelSaveButton : ButtonView
    {
        public ModelTypes ModelType;
        private DataModel model;

        protected override void OnStart()
        {
            base.OnStart();

            model = _bootstrap.GetModel(ModelType) as DataModel;
        }

        protected override void OnClick()
        {
            base.OnClick();

            model.Save();
        }
    }
}