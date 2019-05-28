using UnityEngine;
using UnityEditor;
using InterativaSystem.Models;
using UnityEditorInternal;
using InterativaSystem;
using System.IO;

[CustomEditor(typeof(GridDatabase))]
public class GridDatabaseEditor : Editor
{
    private GridDatabase gridDatabase { get { return target as GridDatabase; } }
    private ReorderableList _list;

    private void OnEnable()
    {
        gridDatabase.IOController.TryLoad(gridDatabase);

        _list = new ReorderableList(serializedObject, serializedObject.FindProperty("gridList"), true, true, true, true);
        _list.onAddCallback += AddElement;
        _list.onRemoveCallback += RemoveElement;
        _list.drawHeaderCallback += DrawHeader;
        _list.drawElementCallback += DrawElement;
    }

    private void OnDisable()
    {
        if (_list == null) return;

        _list.onAddCallback -= AddElement;
        _list.onRemoveCallback -= RemoveElement;
        _list.drawHeaderCallback -= DrawHeader;
        _list.drawElementCallback -= DrawElement;
    }

    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);

        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Obligatory for saving/ loading while editing.", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("IOController"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DataFile"));

        EditorGUILayout.Space();

        if(_list != null)
            _list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void AddElement(ReorderableList list)
    {
        GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow), false, "Edit Grid");
        window.Init(-1, gridDatabase.gridList, gridDatabase);
    }

    void RemoveElement(ReorderableList list)
    {
        gridDatabase.gridList.RemoveAt(list.index);
        gridDatabase.IOController.Save(gridDatabase);
    }

    void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        InterativaSystem.Models.GridLayout item = gridDatabase.gridList[index];

        GUI.Label(new Rect(rect.xMin, rect.y, rect.width - 50f, rect.height), "Controller: " + item.controller.ToString());

        bool b = false;

        if (EditorGUI.Toggle(new Rect(rect.xMax - 20f, rect.y, 50, rect.height), b))
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow), false, "Edit Grid");
            window.Init(index, gridDatabase.gridList, gridDatabase);
        }
    }

    void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Grid Database");
    }
}