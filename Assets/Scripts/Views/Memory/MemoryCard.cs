using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;
using InterativaSystem.Views;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;

namespace Interativa.Views.MemoryGame
{
    [RequireComponent(typeof(LayoutElement))]
    public class MemoryCard : ButtonView
    {
        enum CardState { Open, Closed }

        MemoryController _memoryController;

        RectTransform _transform;
        Image _image;
        LayoutElement _layout;

        CardState state = CardState.Closed;

        int _id;
        public int id { get { return _id; } }

        float _rotationDuration;
        Sprite cardBack;
        Sprite _cardFace;
        public Sprite cardFace
        {
            get { return _cardFace; }
            set
            {
                _cardFace = value;
                _image.sprite = value;
            }
        }

        public bool inactive = false;
        public bool isLocked = false;
        public bool isOpen = false;
        public bool changed = false;
        public bool rotating = false;

        // Use this for initialization
        protected override void OnAwake()
        {
            base.OnAwake();

            _transform = transform.GetChild(0).GetComponent<RectTransform>();
            _image = transform.GetChild(0).GetComponent<Image>();
            _layout = GetComponent<LayoutElement>();

            _layout.preferredWidth = _transform.rect.width;
            _layout.preferredHeight = _transform.rect.height;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _memoryController = _controller as MemoryController;
            _memoryController.RotateCards += Rotate;

            _bootstrap.Reset += ResetCard;

            ResetCard();
        }

        /*private void FixedUpdate()
        {
            if(rotating)
                CheckRotation();
        }*/

        protected override void OnClick()
        {
            if (_memoryController.CheckPaused()) return;
            if (rotating || isLocked || inactive) return;
            LockCard();

            base.OnClick();

            Rotate();
            _memoryController.CheckCard(this);
        }

        public void StartCard()
        {
            _image.sprite = cardFace;
            _transform.localRotation = Quaternion.identity;
        }

        public void Rotate()
        {
            rotating = true;

            isOpen = !isOpen;

            _transform.DOLocalRotate(isOpen ? Vector3.up * 180f : Vector3.zero, _rotationDuration, RotateMode.Fast).
                        SetRelative(false).
                        SetEase(Ease.OutBack).
                        OnUpdate(() => CheckRotation()).
                        OnComplete(() => EndRotation());
        }

        void CheckRotation()
        {
            Vector3 rotation = _transform.localRotation.eulerAngles;

            if ((Mathf.Abs(rotation.y - 90f) <= 45f || Mathf.Abs(rotation.y - 270f) <= 45f) && !changed)
                ChangeImage();

            if (rotation.y >= 270f || rotation.y <= 90f)
                _transform.localScale = Vector3.one;
            else if (rotation.y >= 90f)
                _transform.localScale = new Vector3(-1, 1, 1);
        }

        void EndRotation()
        {
            rotating = false;
        }

        void ChangeImage()
        {
            changed = true;
            StartCoroutine(ResetChange());

            if (_image.sprite == cardFace)
            {
                state = CardState.Closed;
                _image.sprite = cardBack;
            }
            else
            {
                state = CardState.Open;
                _image.sprite = cardFace;
            }
        }

        IEnumerator ResetChange()
        {
            yield return new WaitForSeconds(0.5f);

            changed = false;
        }

        public void SetAttributes(int id, Sprite face, float rotationDuration)
        {
            _id = id;
            cardFace = face;
            _rotationDuration = rotationDuration;
        }

        void ToggleCard()
        {
            _bt.interactable = !_bt.interactable;
        }

        void LockCard()
        {
            isLocked = true;
            _bt.interactable = false;
        }

        void UnlockCard()
        {
            isLocked = false;
            _bt.interactable = true;
        }

        public void RemoveEvents()
        {
            inactive = true;

            //LockCard();
            _bt.interactable = false;
            _memoryController.LockCards -= LockCard;
            _memoryController.UnlockCards -= UnlockCard;
        }

        public void ResetCard()
        {
            inactive = false;
            _bt.interactable = true;
            isOpen = false;

            //_transform.parent.localPosition = Vector3.zero;
            _transform.parent.localRotation = Quaternion.identity;
            _transform.localRotation = Quaternion.identity;
            _transform.parent.localScale = Vector3.one;
            _transform.localScale = Vector3.one;

            _memoryController.LockCards += LockCard;
            _memoryController.UnlockCards += UnlockCard;

            //SetAttributes(_id, cardFace, _rotationDuration);
        }

        public void SetBack(Sprite sprite)
        {
            cardBack = sprite;
        }
    }
}