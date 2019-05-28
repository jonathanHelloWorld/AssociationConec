using System;
using System.Linq;
using InterativaSystem.Models;
using UnityEditor;
using UnityEngine;

namespace InterativaSystem.EditorScripts
{
    [CustomEditor(typeof(ModelsManager))]
    public class ModelsManagerInspector : Editor
    {
        private ModelsManager _modelsManager;
        private MonoScript script;

        void OnEnable()
        {
            _modelsManager = target as ModelsManager;
            //_modelsManager.componentToAdd = MonoScript.FromMonoBehaviour((ModelsManager)target) as GenericModel;
        }

        public override void OnInspectorGUI()
        {
            _modelsManager = target as ModelsManager;
            
            var models = Enum.GetNames(typeof(ModelTypes)).ToList();
            models.Sort();

            EditorGUIUtility.labelWidth = 0.2f;

            if (GUILayout.Button("Populate Base"))
            {
                _modelsManager.Populate();
            }
            EditorGUILayout.Space();

            if (GUILayout.Button(_modelsManager.dropdownMenu ? "/\\" : "\\/"))
            {
                _modelsManager.dropdownMenu = !_modelsManager.dropdownMenu;
            }

            if (_modelsManager.dropdownMenu)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < models.Count; i++)
                {
                    var child = _modelsManager.transform.Find(models[i]);
                    var color = child != null ? Color.white : Color.grey;
                    GUI.backgroundColor = color;

                    if (models[i] != "Null" && GUILayout.Button(models[i]))
                    {
                        if (child != null)
                        {
                            _modelsManager.selectedChild = child;
                        }
                        else
                        {
                            var ch = new GameObject(models[i]);
                            ch.transform.parent = _modelsManager.transform;
                        }
                    }

                    if (i != 0 && (i + 1)%3 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            GUI.backgroundColor = Color.white;

            if (_modelsManager.selectedChild != null)
            {
                EditorGUILayout.LabelField(_modelsManager.selectedChild.name);

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Model", GUILayout.MaxWidth(78)))
                {
                    if (script != null)
                    {
                        _modelsManager.selectedChild.gameObject.AddComponent(script.GetClass());
                        _modelsManager.selectedChild = null;
                        script = null;
                    }
                }
                
                script = EditorGUILayout.ObjectField(script, typeof (MonoScript), false) as MonoScript;

                if (GUILayout.Button("Delete", GUILayout.MaxWidth(80)))
                {
                    DestroyImmediate(_modelsManager.selectedChild.gameObject);
                    _modelsManager.selectedChild = null;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}