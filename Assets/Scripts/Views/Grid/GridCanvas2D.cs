using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InterativaSystem.Enums;
using Interativa.Views.CandyGame;
using System.Linq;
using InterativaSystem.Models;

namespace InterativaSystem.Views.Grid
{
    public class GridCanvas2D
    {
        public Models.GridLayout gridLayout;

        public List<List<GridPiece>> pieces;

        Vector2[] directions = new Vector2[]
        {
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(0,-1),
            new Vector2(-1,0),
        };

        public GridCanvas2D(Models.GridLayout _gridLayout)
        {
            gridLayout = _gridLayout;

            pieces = new List<List<GridPiece>>();

            for (int row = 0; row < gridLayout.grid.Count; row++)
            {
                pieces.Add(new List<GridPiece>());

                for (int col = 0; col < gridLayout.grid[row].Length; col++)
                {
                    pieces[row].Add(new GridPiece());
                }
            }

            CreateGrid();
        }

        public void CreateGrid()
        {
            int count = 0;

            for (int row = 0; row < pieces.Count; row++)
            {
                for (int col = 0; col < pieces[row].Count; col++)
                {
                    pieces[row][col].type = gridLayout.grid[row][col];

                    pieces[row][col].position.x -= -(gridLayout.pieceSize.x + gridLayout.spacing.x) / 2 + ((gridLayout.pieceSize.x + gridLayout.spacing.x) * gridLayout.gridSize.x) / 2 - (gridLayout.pieceSize.x + gridLayout.spacing.x) * col;// - pieceOffset.x;
                    pieces[row][col].position.y -= -(gridLayout.pieceSize.y + gridLayout.spacing.y) / 2 + ((gridLayout.pieceSize.y + gridLayout.spacing.y) * gridLayout.gridSize.y) / 2 - (gridLayout.pieceSize.y + gridLayout.spacing.y) * row;// - pieceOffset.y;
                    pieces[row][col].index = count;

                    pieces[row][col].coodinates.x = row;
                    pieces[row][col].coodinates.y = col;

                    count++;
                }
            }
        }

        public GridPiece[] GetPieces(Direction direction, Vector2 startPos, int numPieces)
        {
            GridPiece[] piecesFound = new GridPiece[numPieces];
            Vector2 dir = directions[(int)direction];

            for (int i = 0; i < numPieces; i++)
            {
                Vector2 newPos = startPos + (dir * (i + 1));
                if (pieces[(int)newPos.x][(int)newPos.y] != null)
                {
                    piecesFound[i] = pieces[(int)(startPos.x + (dir.x * (i + 1)))][(int)(startPos.y + (dir.y * (i + 1)))];
                }
            }

            return piecesFound;
        }

        public Dictionary<string, List<GridPiece>> GetPiecesRow(Vector2 startPos)
        {
            Dictionary<string, List<GridPiece>> dictionary = new Dictionary<string, List<GridPiece>>();

            dictionary.Add("right", new List<GridPiece>());
            dictionary.Add("left", new List<GridPiece>());
            dictionary.Add("top", new List<GridPiece>());
            dictionary.Add("bottom", new List<GridPiece>());

            for(int i = (int)startPos.x + 1; i < gridLayout.gridSize.x; i++)
            {
                if (gridLayout.grid[i][(int)startPos.y] != GridPieceType.Blank)
                    dictionary["top"].Add(pieces[i][(int)startPos.y]);
            }

            for (int i = (int)startPos.x - 1; i >= 0; i--)
            {
                if (gridLayout.grid[i][(int)startPos.y] != GridPieceType.Blank)
                    dictionary["bottom"].Add(pieces[i][(int)startPos.y]);
            }

            for (int i = (int)startPos.y + 1; i < gridLayout.gridSize.y; i++)
            {
                if (gridLayout.grid[(int)startPos.x][i] != GridPieceType.Blank)
                    dictionary["right"].Add(pieces[(int)startPos.x][i]);
            }

            for (int i = (int)startPos.y - 1; i >= 0; i--)
            {
                if (gridLayout.grid[(int)startPos.x][i] != GridPieceType.Blank)
                    dictionary["left"].Add(pieces[(int)startPos.x][i]);
            }

            return dictionary;
        }

        public GridPiece GetPiece(Vector2 position)
        {
            if (pieces.ElementAtOrDefault((int)position.x) != null && pieces[(int)position.x].ElementAtOrDefault((int)position.y) != null &&
                gridLayout.grid[(int)position.x][(int)position.y] != GridPieceType.Blank)
                return pieces[(int)position.x][(int)position.y];
            else
                return null;
        }

        public List<GridPiece> GetAllPieces()
        {
            List<GridPiece> allPieces = new List<GridPiece>();

            for (int i = 0; i < pieces.Count; i++)
            {
                for (int j = 0; j < pieces[i].Count; j++)
                {
                    if(pieces.ElementAtOrDefault(i) != null && pieces[i].ElementAtOrDefault(j) != null &&
                    gridLayout.grid[i][j] != GridPieceType.Blank)
                    allPieces.Add(pieces[i][j]);
                }
            }

            return allPieces;
        }
    }
}