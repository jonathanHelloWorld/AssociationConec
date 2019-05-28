using System.Collections.Generic;
using Assets.Scripts.Extension;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD.App;
using InterativaSystem.Views.HUD.Page;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.EditorScripts
{
    [CustomEditor(typeof(PageManager))]
    public class PageManagerInspector : Editor
    {
        private PageManager _pageManager;

        public override void OnInspectorGUI()
        {
            _pageManager = target as PageManager;

            var labelWidth = EditorGUIUtility.labelWidth;

            if (_pageManager.Pages == null)
            {
                _pageManager.Pages = new List<Page>();
            }
            
            if (GUILayout.Button("Add Page"))
            {
                var temp = new GameObject("Page");
                temp.transform.SetParent(_pageManager.transform);
                temp.AddComponent<Image>();


                temp.GetComponent<RectTransform>().SetUI(new Vector2(0, 0), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0, 0), new Vector2(0.5f,0.5f));

                var child = new GameObject("Next Page Button");
                child.transform.SetParent(temp.transform);
                child.AddComponent<Image>().color = Color.grey;

                child.GetComponent<RectTransform>().SetUI(new Vector2(-40, 40), new Vector2(1, 0), new Vector2(1, 0), new Vector2(256, 72), new Vector2(1, 0));

                var bt = child.AddComponent<PageChangeButton>();
                bt.SetControllerType(ControllerTypes.Page);
                bt.option = PageButtonOption.Next;

                var pg = temp.AddComponent<Page>();
                pg.SetControllerType(ControllerTypes.Page);
                pg.Id = _pageManager.Pages.Count;
                _pageManager.Pages.Add(pg);
            }

            for (int i = 0; i < _pageManager.Pages.Count; i++)
            {
                EditorGUILayout.Space();

                _pageManager.Pages[i].gameObject.name = EditorGUILayout.TextField("Page ", _pageManager.Pages[i].gameObject.name);
                _pageManager.Pages[i].Id = EditorGUILayout.IntField("Id:", _pageManager.Pages[i].Id);
                _pageManager.Pages[i].Type = (PageType)EditorGUILayout.EnumPopup("Type:", _pageManager.Pages[i].Type);

                EditorGUILayout.BeginHorizontal();
                var ge = _pageManager.Pages[i].GetComponent<GamePageNextOnEnd>();
                var pr = _pageManager.Pages[i].GetComponent<AppPageResetAuto>();
                var pa = _pageManager.Pages[i].GetComponent<PageNextAuto>();
                if (ge)
                {
                    EditorGUIUtility.labelWidth = 48f;

                    var type = (ControllerTypes)EditorGUILayout.EnumPopup("Game:", ge.GetControllerType());
                    ge.SetControllerType(type);

                    if (GUILayout.Button("ChangeOnEnd -"))
                    {
                        DestroyImmediate(ge);
                    }

                    EditorGUIUtility.labelWidth = labelWidth;
                }
                else
                {
                    if (GUILayout.Button("ChangeOnEnd +"))
                    {
                        _pageManager.Pages[i].gameObject.AddComponent<GamePageNextOnEnd>();
                    }
                }
                if (pa)
                {
                    pa.SetControllerType(ControllerTypes.Page);
                    if (GUILayout.Button("ChangePageAuto -"))
                    {
                        DestroyImmediate(pa);
                    }
                }
                else
                {
                    if (GUILayout.Button("ChangePageAuto +"))
                    {
                        _pageManager.Pages[i].gameObject.AddComponent<PageNextAuto>();
                    }
                }
                if (pr)
                {
                    pr.SetControllerType(ControllerTypes.Page);
                    if (GUILayout.Button("ResetAuto -"))
                    {
                        DestroyImmediate(pr);
                    }
                }
                else
                {
                    if (GUILayout.Button("ResetAuto +"))
                    {
                        _pageManager.Pages[i].gameObject.AddComponent<AppPageResetAuto>();
                    }
                }


                EditorGUILayout.EndHorizontal();


                if (GUILayout.Button("Remove Page"))
                {
                    DestroyImmediate(_pageManager.Pages[i].gameObject);
                    _pageManager.Pages.RemoveAt(i);
                }
            }
        }
    }
}