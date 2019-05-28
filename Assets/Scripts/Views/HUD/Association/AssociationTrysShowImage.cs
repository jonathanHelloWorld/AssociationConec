using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssociationTrysShowImage : ImageView
    {
        private AssociationController _associationController;
        public int Limit;
        private Sprite iniSprite;
        public Sprite ChangedSprite;

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;

            iniSprite = _image.sprite;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            _image.sprite = _associationController.GetTrys() >= Limit ? ChangedSprite : iniSprite;
        }
    }
}