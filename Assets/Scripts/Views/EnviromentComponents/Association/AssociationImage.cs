using InterativaSystem.Controllers;
using InterativaSystem.Models;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.EnviromentComponents
{
    [RequireComponent(typeof(Image))]
    public class AssociationImage : AssociationObject
    {
        protected Image _image;
        public Sprite Movable, Static;

        protected override void OnAwake()
        {
            base.OnAwake();
            _image = GetComponent<Image>();
            
            GetComponent<BoxCollider>().size = new Vector3(_image.rectTransform.sizeDelta.x, _image.rectTransform.sizeDelta.y, 0.1f);
        }
        
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void EnableStatic()
        {
            base.EnableStatic();

            if (Static != null && Movable != null)
            _image.sprite = IsStatic ? Static : Movable;
        }

        protected override void Associated()
        {
            base.Associated();

            transform.SetSiblingIndex(0);
        }
    }
}