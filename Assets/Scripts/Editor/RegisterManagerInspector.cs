using Assets.Scripts.Views.HUD.Register;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD.Page;
using InterativaSystem.Views.HUD.Register;
using System.Collections.Generic;
using Assets.Scripts.Extension;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.EditorScripts
{
    [CustomEditor(typeof(RegisterManager))]
    public class RegisterManagerInspector : Editor
    {
        private RegisterManager _registerManager;

        void SetUI(RectTransform rect, Vector2 anchoredPosition, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
        {
            rect.localScale = Vector3.one;
            rect.anchoredPosition = anchoredPosition;
            rect.anchorMax = anchorMax;
            rect.anchorMin = anchorMin;
            rect.sizeDelta = sizeDelta;
        }
        public override void OnInspectorGUI()
        {
            _registerManager = target as RegisterManager;

            if (_registerManager.InputFields == null)
            {
                _registerManager.InputFields = new List<RegisterInput>();
            }

            if (GUILayout.Button("Add Input Field"))
            {
                var temp = new GameObject("Resgister Input (Unasigned)");
                temp.transform.SetParent(_registerManager.transform);
                var img = temp.AddComponent<Image>();
                img.color = new Color(0.9f, 0.9f, 0.9f, 1);

                temp.GetComponent<RectTransform>().SetUI(new Vector2(0, 0), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(256, 74), new Vector2(0, 0.5f));

                var input = temp.AddComponent<InputField>();

                var tx = new GameObject("Text");
                tx.transform.SetParent(temp.transform);
                input.textComponent = tx.AddComponent<Text>();
                input.textComponent.supportRichText = false;
                input.textComponent.fontSize = 56;
                input.textComponent.color = Color.black;

                tx.GetComponent<RectTransform>().SetUI(new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-10, -10), new Vector2(0.5f, 0.5f));

                tx = new GameObject("PlaceHolder");
                tx.transform.SetParent(temp.transform);
                input.placeholder = tx.AddComponent<Text>();
                input.placeholder.GetComponent<Text>().supportRichText = false;
                input.placeholder.GetComponent<Text>().text = "...";
                input.placeholder.GetComponent<Text>().fontSize = 56;
                input.placeholder.GetComponent<Text>().color = Color.black;

                tx.GetComponent<RectTransform>().SetUI(new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-10, -10), new Vector2(0.5f, 0.5f));

                tx = new GameObject("Label");
                tx.transform.SetParent(temp.transform);
                var label = tx.AddComponent<Text>();
                label.supportRichText = false;
                label.text = "Unasigned";
                label.fontSize = 62;
                label.color = Color.black;
                label.alignment = TextAnchor.UpperRight;

                tx.GetComponent<RectTransform>().SetUI(new Vector2(-20, 0), new Vector2(0, 0), new Vector2(0, 1), new Vector2(256, 0), new Vector2(1, 0.5f));

                tx = new GameObject("Feedback");
                tx.transform.SetParent(temp.transform);
                var feedback = tx.AddComponent<Image>();
                feedback.color = Color.red;
                feedback.preserveAspect = true;

                tx.GetComponent<RectTransform>().SetUI(new Vector2(30, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(74, 0), new Vector2(0, 0.5f));
                
                var ri = temp.AddComponent<RegisterInput>();
                ri.SetControllerType(ControllerTypes.Register);
                ri.input = input;
                ri.Label = label;
                ri.CheckFeddback = feedback.gameObject;
                _registerManager.InputFields.Add(ri);
            }

            for (int i = 0; i < _registerManager.InputFields.Count; i++)
            {
                EditorGUILayout.Space();

                _registerManager.InputFields[i].DataName = EditorGUILayout.TextField("Input Name ", _registerManager.InputFields[i].DataName);
                _registerManager.InputFields[i].gameObject.name = "Input Field (" + _registerManager.InputFields[i].DataName + ")";
                _registerManager.InputFields[i].Label.text = _registerManager.InputFields[i].DataName + ":";
                _registerManager.InputFields[i].IsFirstSelected = EditorGUILayout.Toggle("Is First Selected", _registerManager.InputFields[i].IsFirstSelected);
                _registerManager.InputFields[i].IsUnique = EditorGUILayout.Toggle("Is Unique", _registerManager.InputFields[i].IsUnique);
                _registerManager.InputFields[i].CheckType = (CheckType)EditorGUILayout.EnumPopup("Type:", _registerManager.InputFields[i].CheckType);

                if (GUILayout.Button("Remove Input Field"))
                {
                    DestroyImmediate(_registerManager.InputFields[i].gameObject);
                    _registerManager.InputFields.RemoveAt(i);
                }
            }
        }
    }
}