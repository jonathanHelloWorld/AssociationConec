using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using UnityEngine.UI;
using InterativaSystem.Controllers;
using DG.Tweening;

namespace Interativa.Views.CandyGame
{
    public class CandyComboText : GenericView
    {
        public float duration = 0.5f;
        public float offset = 1f;

        Transform _transform;
        Text _text;

        protected override void OnStart()
        {
            base.OnStart();

            _transform = GetComponent<Transform>();
            _text = GetComponent<Text>();

            ((CandyController)_controller).comboText = this;

            gameObject.SetActive(false);
        }

        public void ShowText(string text, Vector3 startPosition)
        {
            gameObject.SetActive(true);

            _transform.SetAsLastSibling();

            _transform.DOKill(false);
            _transform.localPosition = startPosition;
            _transform.localScale = Vector3.zero;

            _text.text = "x" + text;

            _transform.DOScale(Vector3.one, duration / 2).SetEase(Ease.OutBounce).SetLoops(1, LoopType.Yoyo).Play();
            _transform.DOLocalMoveY(startPosition.y + offset, duration).OnComplete(() => SetOff()).Play();
        }

        public void SetOff()
        {
            gameObject.SetActive(false);
        }
    }
}