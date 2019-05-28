using System;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Views.HUD.ActionCall
{
    public class CallColliderOnById : ColliderView
    {
        [Space]
        public int Id;
        public List<int> Ids;
        public bool NeverHide;
        public bool EnableOnLastActionEnded;
        public bool OnlyReturnOnActionEnded;
        public bool DisableOnActionStart;

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += Hide;

            if (DisableOnActionStart)
                _controller.CallGenericAction += Hide;

            if (EnableOnLastActionEnded)
                _controller.GenericActionEnded += ShowBefore;

            if (Ids != null && Ids.Count > 0)
            {
                _collider.enabled = Ids.Contains(0);
            }
            else
            {
                _collider.enabled = Id == 0;
            }
        }
        protected override void OnUpdate() { }

        protected virtual void In() { }
        protected virtual void Out() { }

        private void ShowBefore(int value)
        {
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Exists(x => x - 1 != value)) return;
            }
            else
            {
                if (Id - 1 != value) return;
            }

            _collider.enabled = true;

            In();
        }
        private void CheckShow(int value)
        {
            var result = Id == value;
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Exists(x => NeverHide && x != value)) return;
                result = Ids.Contains(value);
            }
            else
            {
                if (NeverHide && Id != value) return;
                result = Id == value;
            }

            if (!result && OnlyReturnOnActionEnded) return;

            _collider.enabled = result;

            if (result)
                In();
            else
                Out();
        }
        private void Hide(int value)
        {
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Exists(x => NeverHide || x != value)) return;
            }
            else
            {
                if (Id != value || NeverHide) return;
            }

            _collider.enabled = false;

            Out();
        }
    }
}