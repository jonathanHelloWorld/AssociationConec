using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Mosaic
{
    public class MosaicView : ImageView
    {
        public Vector3 iniPos;
        public Transform HighlightPos, OutPos, Root;
        
        public void SetColor(Color color)
        {
            _image.color = color;
        }

        public void SetImage(Sprite spr)
        {
            _image.color = Color.white;
            _image.sprite = spr;
        }

        private const float AnimationTime = 2;
        public void Animate(Sprite spr, int id)
        {
            transform.position = OutPos.position;
            _image.color = Color.white;
            _image.sprite = spr;
            
            transform.DOMove(HighlightPos.position, AnimationTime).SetDelay(AnimationTime*id).SetEase(Ease.OutCubic).Play();
            transform.DOMove(iniPos, AnimationTime).SetDelay(AnimationTime + AnimationTime * id).SetEase(Ease.InCubic).Play().OnComplete(SetThisParent);
        }

        void SetThisParent()
        {
            transform.SetParent(Root);
        }
    }
}