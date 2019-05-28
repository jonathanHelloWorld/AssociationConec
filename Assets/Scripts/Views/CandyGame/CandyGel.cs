using UnityEngine;
using System.Collections;
using DG.Tweening;
using InterativaSystem.Views.Grid;
using InterativaSystem.Views;
using InterativaSystem.Controllers;

namespace Interativa.Views.CandyGame
{
    public class CandyGel : GenericView
    {
        public Animator _animator;
        public SpriteRenderer _sprite;

        public GridPiece gridPiece;

        public string animToDestroy = "toDestroy";
        public bool isDestroyed = true;

        protected override void OnStart()
        {
            base.OnStart();

            ((CandyController)_controller).KillGels += ToDestroy;
            ((CandyController)_controller).ShowGels += BringToFront;
        }

        public void ToDestroy()
        {
            isDestroyed = true;
            _animator.SetBool(animToDestroy, isDestroyed);
        }

        public void Reset()
        {
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;

            isDestroyed = false;
            _animator.SetBool(animToDestroy, isDestroyed);
        }

        public void BringToFront()
        {
            foreach (SpriteRenderer _renderer in GetComponentsInChildren<SpriteRenderer>())
            {
                _renderer.sortingOrder = 1;
            }
        }

        public void SendToBack()
        {
            foreach (SpriteRenderer _renderer in GetComponentsInChildren<SpriteRenderer>())
            {
                _renderer.sortingOrder = 0;
            }
        }
    }
}