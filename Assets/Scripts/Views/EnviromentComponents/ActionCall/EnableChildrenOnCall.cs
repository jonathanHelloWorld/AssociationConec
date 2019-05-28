using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.ActionCall
{
    public class EnableChildrenOnCall : GenericView
    {
        [Space] public float Delay;

        public int Id;
        public List<int> Ids;
        public bool EnableOnLastActionEnded;
        public bool OnlyReturnOnActionEnded;
        public bool DisableOnActionStart;

        private List<GameObject> childs;
        
        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += Disable;

            if (DisableOnActionStart)
                _controller.CallGenericAction += Disable;

            if (EnableOnLastActionEnded)
                _controller.GenericActionEnded += ShowBefore;

            childs = new List<GameObject>();
            for (int i = 0, n = transform.childCount; i < n; i++)
            {
                childs.Add(transform.GetChild(i).gameObject);
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        private void ShowBefore(int value)
        {
            if (Id - 1 != value) return;

            StartCoroutine(Enable(value));
        }

        private void CheckShow(int value)
        {
            //if (Ids.Count > 0 && Ids.Contains(value) && Id != value) return;

            if (Ids.Count > 0 && Ids.Contains(value) || Id == value)
                StartCoroutine(Enable(value));
            else if (!OnlyReturnOnActionEnded)
                Disable(value);
        }

        IEnumerator Enable(int value)
        {
            yield return new WaitForSeconds(Delay);

            if (Ids.Count > 0)
            {
                for (int i = 0, n = childs.Count; i < n; i++)
                {
                    childs[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0, n = childs.Count; i < n; i++)
                {
                    childs[i].SetActive(true);
                }
            }
        }

        public void Disable(int value)
        {
            //if (Id != value) return;

            for (int i = 0, n = childs.Count; i < n; i++)
            {
                childs[i].SetActive(false);
            }
        }
    }
}