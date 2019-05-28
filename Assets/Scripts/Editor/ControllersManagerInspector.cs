using System;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEditor;
using UnityEngine;

namespace InterativaSystem.EditorScripts
{
    [CustomEditor(typeof(ControllersManager))]
    public class ControllersManagerInspector : Editor
    {
        private ControllersManager _controllersManager;

        public override void OnInspectorGUI()
        {
            _controllersManager = target as ControllersManager;
            var controllers = Enum.GetNames(typeof(ControllerTypes)).ToList();
            controllers.Sort();

            EditorGUIUtility.labelWidth = 0.2f;

            if (GUILayout.Button("Populate Base"))
            {
                _controllersManager.Populate();
            }
            EditorGUILayout.Space();

            if (GUILayout.Button(_controllersManager.dropdownMenu ? "/\\" : "\\/"))
            {
                _controllersManager.dropdownMenu = !_controllersManager.dropdownMenu;
            }

            if (_controllersManager.dropdownMenu)
            {
                EditorGUILayout.BeginHorizontal();




                for (int i = 0; i < controllers.Count; i++)
                {
                    var child = _controllersManager.transform.Find(controllers[i]);
                    var color = child != null ? Color.white : Color.grey;
                    GUI.backgroundColor = color;

                    if (controllers[i] != "Null" && GUILayout.Button(controllers[i]))
                    {
                        if (child != null)
                        {
                            _controllersManager.selectedChild = child;
                        }
                        else
                        {
                            var ch = new GameObject(controllers[i]);
                            ch.transform.parent = _controllersManager.transform;
                        }
                    }

                    if (i != 0 && (i + 1) % 3 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            GUI.backgroundColor = Color.white;

            if (_controllersManager.selectedChild != null)
            {
                EditorGUILayout.LabelField(_controllersManager.selectedChild.name);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Controller", GUILayout.MaxWidth(120)))
                {
                    if (script != null)
                    {
                        _controllersManager.selectedChild.gameObject.AddComponent(script.GetClass());
                        _controllersManager.selectedChild = null;
                        script = null;
                    }
                }

                script = EditorGUILayout.ObjectField(script, typeof (MonoScript), false) as MonoScript;

                if (GUILayout.Button("Delete", GUILayout.MaxWidth(80)))
                {
                    DestroyImmediate(_controllersManager.selectedChild.gameObject);
                    _controllersManager.selectedChild = null;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private MonoScript script;
    }
}