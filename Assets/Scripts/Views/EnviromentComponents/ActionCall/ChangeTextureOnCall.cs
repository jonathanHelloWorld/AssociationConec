using System.Collections.Generic;
using DG.Tweening;
using InterativaSystem.Views.HUD;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.ActionCall
{
    public class ChangeTextureOnCall : RendererView
    {
        public Texture Target;
        public Texture[] Targets;
        private Texture _default;
        [Space]
        public int Id;
        public List<int> Ids;
        public bool EnableOnLastActionEnded;
        public bool DisableOnActionStart;

        private Material _material;

        protected override void OnStart()
        {
            base.OnStart();

            _material = _renderer.material;

            _default = _material.mainTexture;


            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += GoBack;

            if (DisableOnActionStart)
                _controller.CallGenericAction += GoBack;

            if (EnableOnLastActionEnded)
                _controller.GenericActionEnded += ShowBefore;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        private void ShowBefore(int value)
        {
            if (Id - 1 != value) return;

            Go(value);
        }

        private void CheckShow(int value)
        {
            //if (Ids.Count > 0 && Ids.Contains(value) && Id != value) return;

            if (Ids.Count > 0 && Ids.Contains(value) || Id == value)
                Go(value);
            else
                GoBack(value);
        }

        public void Go(int value)
        {
            if (Ids.Count > 0)
            {
                var id = Ids.FindIndex(x => x == value);
                _material.mainTexture = Targets[id];
            }
            else
            {
                _material.mainTexture = Target;
            }
        }

        public void GoBack(int value)
        {
            _material.mainTexture = _default;
        }
    }
}