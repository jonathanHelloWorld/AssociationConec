using InterativaSystem.Views.HUD.Quiz;
using System.Collections.Generic;
using Assets.Scripts.Extension;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD.Question;
using InterativaSystem.Views.HUD.Register;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.EditorScripts
{
    [CustomEditor(typeof(QuizManager))]
    public class QuizManagerInspector : Editor
    {
        private QuizManager _quizManager;

        public override void OnInspectorGUI()
        {
            _quizManager = target as QuizManager;

            if (_quizManager.alternatives == null)
            {
                _quizManager.alternatives = new List<QuestionAlternativeText>();
            }

            if (_quizManager.tittle == null)
            {
                if (GUILayout.Button("Add Tittle"))
                {
                    var temp = new GameObject("Tittle");
                    temp.transform.SetParent(_quizManager.transform);
                    var tx = temp.AddComponent<Text>();
                    tx.color = Color.black;
                    var tit = _quizManager.tittle = temp.AddComponent<QuestionTitleText>();
                    tit.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    tit.GetComponent<Text>().resizeTextForBestFit = true;
                    tit.GetComponent<Text>().resizeTextMinSize = 14;
                    tit.GetComponent<Text>().resizeTextMaxSize = 86;

                    tit.SetControllerType(ControllerTypes.Quiz);

                    temp.GetComponent<RectTransform>().SetUI(new Vector2(0, -20), new Vector2(0, 1), new Vector2(1, 1), new Vector2(-40, 120), new Vector2(0.5f, 1));
                }
            }
            else
            {
                if (GUILayout.Button("Remove Tittle"))
                {
                    DestroyImmediate(_quizManager.tittle.gameObject);
                    _quizManager.tittle = null;
                }
            }

            EditorGUILayout.Space();

            if (_quizManager.transform.Find("Vertical") == null)
            {
                var temp = new GameObject("Vertical");
                temp.transform.SetParent(_quizManager.transform);
                var tx = temp.AddComponent<Text>();
                tx.color = Color.black;
                var ag = temp.AddComponent<VerticalLayoutGroup>();
                ag.spacing = 5;

                temp.GetComponent<RectTransform>().SetUI(new Vector2(0, 160), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-40, -20), new Vector2(0.5f, 0.5f));
            }
            else
            {
                var vert = _quizManager.transform.Find("Vertical");
                if (GUILayout.Button("Add Alternative"))
                {
                    var temp = new GameObject("Alternative (" + _quizManager.alternatives.Count + ")");

                    temp.transform.SetParent(vert.transform);

                    var img = temp.AddComponent<Image>();
                    img.color = Color.white;
                    temp.GetComponent<RectTransform>().localScale = Vector3.one;

                    temp.AddComponent<Button>();
                    var sb = temp.AddComponent<QuizAswerButton>();
                    sb.SetControllerType(ControllerTypes.Quiz);
                    sb.Id = _quizManager.alternatives.Count;

                    var txTemp = new GameObject("Label");
                    txTemp.transform.SetParent(temp.transform);

                    var tx = txTemp.AddComponent<Text>();
                    tx.alignment = TextAnchor.MiddleCenter;
                    tx.resizeTextForBestFit = true;
                    tx.resizeTextMinSize = 14;
                    tx.resizeTextMaxSize = 64;
                    tx.color = Color.black;
                    tx.text = (_quizManager.alternatives.Count + 1).ToString();
                    txTemp.GetComponent<RectTransform>().SetUI(new Vector2(5, 0), new Vector2(0, 0), new Vector2(0, 1), new Vector2(90, -10), new Vector2(0, 0.5f));

                    txTemp = new GameObject("Alternative Text");
                    txTemp.transform.SetParent(temp.transform);

                    tx = txTemp.AddComponent<Text>();
                    tx.alignment = TextAnchor.MiddleLeft;
                    tx.resizeTextForBestFit = true;
                    tx.resizeTextMinSize = 14;
                    tx.resizeTextMaxSize = 64;
                    tx.color = Color.black;

                    txTemp.GetComponent<RectTransform>().localScale = Vector3.one;
                    var ri = txTemp.AddComponent<QuestionAlternativeText>();

                    txTemp.GetComponent<RectTransform>().SetUI(new Vector2(50, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(-110, -10), new Vector2(0.5f, 0.5f));

                    ri.SetControllerType(ControllerTypes.Register);
                    ri.Id = _quizManager.alternatives.Count;
                    _quizManager.alternatives.Add(ri);
                }
            }

            /*
            for (int i = 0; i < _quizManager.alternatives.Count; i++)
            {
                EditorGUILayout.Space();

                _quizManager.InputFields[i].DataName = EditorGUILayout.TextField("Input Name ", _quizManager.InputFields[i].DataName);
                _quizManager.InputFields[i].gameObject.name = "Input Field (" + _quizManager.InputFields[i].DataName + ")";
                _quizManager.InputFields[i].Label.text = _quizManager.InputFields[i].DataName + ":";
                _quizManager.InputFields[i].IsFirstSelected = EditorGUILayout.Toggle("Is First Selected", _quizManager.InputFields[i].IsFirstSelected);
                _quizManager.InputFields[i].IsUnique = EditorGUILayout.Toggle("Is Unique", _quizManager.InputFields[i].IsUnique);
                _quizManager.InputFields[i].CheckType = (CheckType)EditorGUILayout.EnumPopup("Type:", _quizManager.InputFields[i].CheckType);
            }
            /**/

            EditorGUILayout.LabelField(_quizManager.alternatives.Count.ToString("00") + " Alternatives");
            var i = _quizManager.alternatives.Count - 1;
            if (i >= 0 && GUILayout.Button("Remove last alternative"))
            {
                DestroyImmediate(_quizManager.alternatives[i].transform.parent.gameObject);
                _quizManager.alternatives.RemoveAt(i);
            }
        }
    }
}