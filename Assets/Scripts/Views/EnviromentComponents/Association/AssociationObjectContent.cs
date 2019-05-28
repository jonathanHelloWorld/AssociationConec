using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class AssociationObjectContent : GenericView
    {
        private AssociationController _associationController;
        public CanvasGroup ParentCanvas;

        public int Type;

        [HideInInspector]
        public int Id;

        private List<AssociationObject> _associationObjects;

        private ResourcesDataBase _resources;

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;
            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            _associationController.AddAssociationObjectContent(this);

            ResetContent();
        }

        public void AddContent(int i)
        {
            Debug.Log(i);
            var temp = Instantiate(Resources.Load<AssociationObject>(_resources.Prefabs.Find(x => x.id == i && x.category == PrefabCategory.AssociationObjectStatic).name));
            temp.transform.parent = this.transform;
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = Vector3.one;
            temp.Type = Type;

            if (temp.GetComponent<ColliderView>() != null) 
                temp.GetComponent<ColliderView>().ParentCanvas = ParentCanvas;

            temp.Initialize();

            _associationObjects.Add(temp);
        }

        protected override void ResetFromBootstrap()
        {
            base.ResetFromBootstrap();
        }

        protected override void ResetView()
        {
            base.ResetView();

            ResetContent();
        }

        void ResetContent()
        {
            if (_associationObjects != null)
            {
                for (int i = 0, n = _associationObjects.Count; i < n; i++)
                {
                    Destroy(_associationObjects[i].gameObject);
                }
            }

            _associationObjects = new List<AssociationObject>();
        }
    }
}