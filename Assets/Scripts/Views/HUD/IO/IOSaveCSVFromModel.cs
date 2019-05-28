using System.IO;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.IO
{
    public class IOSaveCSVFromModel : ButtonView
    {
        public ModelTypes Model;
        public string SavePath;
        public string FileName;
        private DataModel model;

        protected override void OnStart()
        {
            base.OnStart();

            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);


            model = _bootstrap.GetModel(Model) as DataModel;
        }

        private void Save()
        {
            File.WriteAllText(SavePath + FileName, model.SerializeDataBaseToCSV());
        }

        protected override void OnClick()
        {
            if (_bootstrap.IsMobile) return;

            base.OnClick();

            Save();
        }
    }
}