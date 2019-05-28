using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Views.HUD
{
    public class ColliderCheckOnMultipleCanvas : ColliderView
    {
        public List<CanvasGroup> Canvas;

        protected override void OnUpdate()
        {
            base.OnUpdate();


            _collider.enabled = Canvas.Exists(x=>x.interactable);
        }
    }
}