using UnityEngine;
using System.Collections;
using UnityEditor;
using InterativaSystem.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using InterativaSystem;

public class GridWindow : EditorWindow
{
    GridDatabase _gridDatabase;
    List<InterativaSystem.Models.GridLayout> _gridList;
    InterativaSystem.Models.GridLayout _gridLayout;
    List<GridPieceType[]> _oldGrid;
    int _index;

    //float _drawScale = 1f;

    bool HasInitialized = false;

    public void Init(int index, List<InterativaSystem.Models.GridLayout> gridList, GridDatabase db)
    {
        _gridDatabase = db;

        _index = index;
        _gridList = gridList;
        if (index >= 0)
            _gridLayout = new InterativaSystem.Models.GridLayout(_gridList[index]);
        else
            _gridLayout = new InterativaSystem.Models.GridLayout();

        ResetList();

        HasInitialized = true;
    }

    void OnGUI()
    {
        if (!HasInitialized) return;

        EditorUtility.SetDirty(this);

        _gridLayout.controller = (ControllerTypes)EditorGUI.EnumPopup(new Rect(0f, 10f, 200f, 20f), "Controller", _gridLayout.controller);

        EditorGUI.BeginChangeCheck();
        _gridLayout.gridSize = EditorGUI.Vector2Field(new Rect(0f, 40f, 200f, 20f), "Grid Size", _gridLayout.gridSize);
        if (EditorGUI.EndChangeCheck())
        {
            ResetList();
        }

        _gridLayout.pieceSize = EditorGUI.Vector2Field(new Rect(0f, 80f, 200f, 20f), "Piece Size", _gridLayout.pieceSize);
        _gridLayout.spacing = EditorGUI.Vector2Field(new Rect(0f, 120f, 200f, 20f), "Piece Spacing", _gridLayout.spacing);

        GUI.Label(new Rect(0f, 160f, 200f, 20f), "Scale");
        _gridLayout.editorScale = GUI.HorizontalSlider(new Rect(20f, 170f, 180f, 20f), _gridLayout.editorScale, 0.1f, 1f);

        if (GUI.Button(new Rect(20f, 210f, 180f, 20f), "Save", GUI.skin.button))
        {
            if (_index >= 0)
                _gridDatabase.gridList[_index] = _gridLayout;
            else
                _gridDatabase.gridList.Add(_gridLayout);

            _gridDatabase.IOController.Save(_gridDatabase);

            Close();
        }

        Handles.BeginGUI();
        Handles.color = Color.grey;
        Handles.DrawLine(new Vector3(210f, 0f), new Vector3(210f, position.height));
        Handles.EndGUI();

        DrawGrid();
    }

    void ResetList()
    {
        _oldGrid = _gridLayout.grid;
        _gridLayout.grid = new List<GridPieceType[]>();

        for (int y = 0; y < _gridLayout.gridSize.y; y++)
        {
            _gridLayout.grid.Add(new GridPieceType[(int)_gridLayout.gridSize.x]);
            for (int x = 0; x < _gridLayout.gridSize.x; x++)
            {
                if (_oldGrid != null && _oldGrid.ElementAtOrDefault(y) != null && x < _oldGrid[y].Length)
                    _gridLayout.grid[y][x] = _oldGrid[y][x];
                else
                    _gridLayout.grid[y][x] = GridPieceType.Normal;
            }
        }
    }

    void DrawGrid()
    {
        Vector2 pos = Vector2.zero;

        for(int y = 0; y < _gridLayout.gridSize.y; y++)
        {
            for (int x = 0; x < _gridLayout.gridSize.x; x++)
            {
                pos.x = 250f + (_gridLayout.pieceSize.x * x + _gridLayout.spacing.x * x) * _gridLayout.editorScale;
                pos.y = 25 + (_gridLayout.pieceSize.y * y + _gridLayout.spacing.y * y) * _gridLayout.editorScale;

                bool check = _gridLayout.grid != null ? _gridLayout.grid.ElementAtOrDefault(y) != null : false;
                GUIStyle style = new GUIStyle(GUI.skin.button);

                if (check)
                {
                    switch (_gridLayout.grid[y][x])
                    {
                        case GridPieceType.Blank:
                            style.normal.background = MakeTex((int)_gridLayout.gridSize.x, (int)_gridLayout.gridSize.y, new Color(0.3f, 0.3f, 0.3f));
                            break;
                        case GridPieceType.CandyGel:
                            style.normal.background = MakeTex((int)_gridLayout.gridSize.x, (int)_gridLayout.gridSize.y, new Color(0, 0.8f, 0.8f));
                            break;
                        default:
                            style.normal.background = MakeTex((int)_gridLayout.gridSize.x, (int)_gridLayout.gridSize.y, new Color(0.9f, 0.9f, 0.9f));
                            break;
                    }
                }

                if (GUI.Button(new Rect(pos.x, pos.y, _gridLayout.pieceSize.x * _gridLayout.editorScale, _gridLayout.pieceSize.y * _gridLayout.editorScale), check ? _gridLayout.grid[y][x].ToString().Substring(0, 1) : "N", style))
                {
                    int v = (int)_gridLayout.grid[y][x];
                    v++;

                    if (v > (int)Enum.GetValues(typeof(GridPieceType)).Cast<GridPieceType>().Max())
                        v = 0;

                    _gridLayout.grid[y][x] = (GridPieceType)v;
                }
            }
        }
    }

    public void ShowWindow()
    {
        minSize = Vector2.one * 600f;
        GetWindow(typeof(GridWindow), false, "Edit Grid");
    }

    Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}