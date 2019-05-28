using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views;
using InterativaSystem.Views.Grid;
using Newtonsoft.Json;


namespace InterativaSystem.Models
{
    [RequireComponent(typeof (BoxCollider))]
    [AddComponentMenu("ModularSystem/DataModel/ Groups")]
    public class GridData : DataModel
    {
        public GridType GridType = GridType.Quad;
        
        private BoxCollider gridArea;
        public int GridQnt;
        public Views.Grid.Grid grid;
        [HideInInspector] public bool gridGenerated;
        public bool useHeights;
        public LayerMask mask;
        public event GenericController.SimpleEvent GridGenerated;
        
        private void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Grid;

            if (!OverrideDB)
                grid = new Views.Grid.Grid();
        }

        protected override void OnStart()
        {
            base.OnStart();

            gridArea = GetComponent<BoxCollider>();

            if (!OverrideDB && !_IOController.TryLoad(this))
            {
                CreateGrid();
                _IOController.Save(this);
            }
            else if (OverrideDB)
            {
                CreateGrid();
                _IOController.Save(this);
            }

            gridArea.enabled = false;
        }

        private void CreateGrid()
        {
            gridGenerated = false;
            switch (GridType)
            {
                case GridType.Quad:
                    grid = new SquareGrid();
                    break;
                case GridType.Hex:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var size = Vector3.Scale(gridArea.size, transform.localScale);
            var pos = transform.position;

            grid.CreateGrid(size, pos, GridQnt, useHeights, mask);

            grid.PrintGrid();
            Debug.Log("Usables grid Count: " + grid.CountValid());

            if (GridGenerated != null) GridGenerated();
            gridGenerated = true;
        }

        private void LoadGrid()
        {

        }

        //This methods will serialize an deserialize from a json data
        public override string SerializeDataBase()
        {
            return "";
        }
        public override void DeserializeDataBase(string json)
        {
            grid = null;
        }
    }
}
