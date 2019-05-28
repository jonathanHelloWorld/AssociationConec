using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using InterativaSystem.Models;

namespace InterativaSystem.Views.Grid
{
    public class GridPiece
    {
        public int index;
        public Vector3 position = Vector3.zero;
        public Vector2 coodinates = Vector2.zero;
        public GenericView objScript;

        public GridPieceType type = GridPieceType.Blank;
    }
}