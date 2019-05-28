using UnityEngine;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using InterativaSystem.Controllers;

namespace InterativaSystem.Models
{
    public class GridDatabase : DataModel
    {
        public IOController IOController;
        public List<GridLayout> gridList;

        void Awake()
        {
            Type = ModelTypes.Grid;
        }

        protected override void OnStart()
        {
            if (!_IOController.TryLoad(this))
            {
                _IOController.Save(this);
            }
        }

        public List<GridLayout> GetAllGrids()
        {
            return gridList;
        }

        public List<GridLayout> GetGridsByGame(ControllerTypes type)
        {
            return gridList.FindAll(x => x.controller.Equals(type));
        }

        #region Serialization
        public override string SerializeDataBase()
        {
            return JsonConvert.SerializeObject(gridList, Formatting.Indented);
        }
        public override void DeserializeDataBase(string json)
        {
            gridList = JsonConvert.DeserializeObject<List<GridLayout>>(json);
        }
        #endregion
    }

    public enum GridPieceType
    {
        Normal,
        Blank,
        CandyGel
    }

    [Serializable]
    public class GridLayout
    {
        public ControllerTypes controller = ControllerTypes.Null;
        public Vector2 gridSize = Vector2.zero;
        public Vector2 pieceSize = Vector2.zero;
        public Vector2 spacing = Vector2.zero;
        public List<GridPieceType[]> grid;
        public float editorScale = 1;

        public GridLayout() { }
        public GridLayout(GridLayout copyFrom)
        {
            controller = copyFrom.controller;
            gridSize = copyFrom.gridSize;
            pieceSize = copyFrom.pieceSize;
            spacing = copyFrom.spacing;
            grid = copyFrom.grid;
            editorScale = copyFrom.editorScale;
        }
    }
}