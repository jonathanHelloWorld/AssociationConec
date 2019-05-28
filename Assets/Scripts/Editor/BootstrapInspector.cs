using System;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterativaSystem.EditorScripts
{
    [CustomEditor(typeof(Bootstrap))]
    public class BootstrapInspector : Editor
    {
        protected Bootstrap _bootstrap;

        private void OnEnable()
        {
#if (HAS_SERVER && UNITY_IOS)
            PlayerSettings.iOS.requiresPersistentWiFi = true;
#endif
        }
        
        public override void OnInspectorGUI()
        {
            _bootstrap = target as Bootstrap;
            
            EditorGUILayout.LabelField("Load Scenes", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            _bootstrap.LoadScenesOnEditor = EditorGUILayout.Toggle("On Editor Startup", _bootstrap.LoadScenesOnEditor);
            _bootstrap.LoadScenesOnStartup = EditorGUILayout.Toggle("On Startup", _bootstrap.LoadScenesOnStartup);
            EditorGUILayout.EndHorizontal();

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("ScenesToAddOnStart");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();

            //_bootstrap.ScenesToAddOnStart = EditorGUILayout.li("On Startup", _bootstrap.LoadScenesOnStartup);

            EditorGUILayout.Space();

            _bootstrap.DebugOnEdtor = EditorGUILayout.Toggle("Debug On Edtor", _bootstrap.DebugOnEdtor);

            if (GUILayout.Button("Create Hierarchy"))
            {
                CreateHierarchy();
            }

        }

        private void CreateHierarchy()
        {
            _bootstrap.transform.position = Vector3.zero;

            var controllers = Enum.GetNames(typeof(ControllerTypes)).ToList();
            controllers.Sort();

            var temp = new GameObject("Controllers");
            temp.AddComponent<ControllersManager>();
            temp.transform.parent = _bootstrap.transform;

            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i] == "null") continue;
                var ch = new GameObject(controllers[i]);
                ch.transform.parent = temp.transform;
            }

            var models = Enum.GetNames(typeof(ModelTypes)).ToList();
            models.Sort();
            temp = new GameObject("Models");
            temp.AddComponent<ModelsManager>();
            temp.transform.parent = _bootstrap.transform;

            for (int i = 0; i < models.Count; i++)
            {
                if (models[i] == "null") continue;
                var ch = new GameObject(models[i]);
                ch.transform.parent = temp.transform;
            }

            temp = new GameObject("EventSystem");
            temp.AddComponent<EventSystem>();
            temp.AddComponent<StandaloneInputModule>();
            temp.transform.parent = _bootstrap.transform;

            temp = new GameObject("Canvas HUD");
            var canvas = temp.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = temp.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            temp.AddComponent<GraphicRaycaster>();
            temp.transform.parent = _bootstrap.transform;

            temp = new GameObject("2D World");
            temp.transform.parent = _bootstrap.transform;
            temp = new GameObject("3D World");
            temp.transform.parent = _bootstrap.transform;
        }
    }
}