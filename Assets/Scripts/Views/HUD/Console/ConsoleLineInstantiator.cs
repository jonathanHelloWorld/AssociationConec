using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Views.HUD.Console
{
    public class ConsoleLineInstantiator : GenericView
    {
        private ConsoleController _console;
        private ResourcesDataBase _resources;

        public Scrollbar Scrollbar;

        protected override void OnStart()
        {
            _console = _controller as ConsoleController;
            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            if (_console != null)
                _console.ConsolePrint += AddLine;
        }

        private void AddLine(string value)
        {
            var temp = Instantiate(Resources.Load<GameObject>(_resources.Prefabs.Find(x => x.category == PrefabCategory.UILineText).name));

            temp.transform.SetParent(this.transform);
            temp.transform.localScale = Vector3.one;
            temp.transform.localPosition = Vector3.zero;
            temp.GetComponent<Text>().text = value;

            if (Scrollbar != null)
                Scrollbar.value = 0;
        }
    }
}