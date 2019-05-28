using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.EnviromentComponents;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssociationSpriteChangeById : ImageView
    {
        public Sprite[] SpritesNormal;
        public Sprite[] SpritesAssociated;


        private bool _isAssociated;
        private AssociationController _associationController;

        [Space]
        public AssociationObject Parent;

        [HideInInspector]
        public int ReferenceId;

        protected override void OnAwake()
        {
            base.OnAwake();

            if (Parent.IsStatic)
                this.enabled = false;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;
            _associationController.OnGamePrepare += GetId;
            _associationController.OnAssociation += GetId;

            GetId();
        }

        private void GetId()
        {
            ReferenceId = Parent.QuestionId;
            _isAssociated = Parent.IsAssociated;

            SetImage();
        }

        void SetImage()
        {
            _image.sprite = _isAssociated ? SpritesAssociated[ReferenceId] : SpritesNormal[ReferenceId];
        }
    }
}