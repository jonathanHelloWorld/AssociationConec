using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.Grid
{
    public class GetGridPointByMouse : GenericView
    {
        public LayerMask mask;

        private ConquerController conquerController;
        private GridData gridInfo;
        private GridPoint selectedGrid;
        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
            gridInfo = _bootstrap.GetModel(ModelTypes.Grid) as GridData;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, mask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.blue, 2);
                selectedGrid = gridInfo.grid.GetGridPoint(hit.point, gridInfo.grid.PointSize/2);
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (selectedGrid != null)
                {
                    Debug.DrawRay(selectedGrid.worldPosition, Vector3.up*10, Color.red, 2);
                    conquerController.ActualGrid = selectedGrid;
                }
            }
        }
    }
}