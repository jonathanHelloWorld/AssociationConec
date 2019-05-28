using System.Collections.Generic;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.Input
{
    public class ChildOnByInputToogle : GenericView
    {
        public int Id;
        protected InputController _inputController;

        private List<GameObject> childs; 
        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;

            childs = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                childs.Add(transform.GetChild(i).gameObject);
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_inputController.ToogleControllers.Count < Id) return;


            for (int i = 0, n = childs.Count; i < n; i++)
            {
                childs[i].SetActive(_inputController.ToogleControllers[Id]);
            }
        }
    }
}