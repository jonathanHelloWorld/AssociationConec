using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Association
{
    public class AssociationChangeImageOnRound : ImageView
    {
        public Sprite[] Images;
        private AssociationController _associationController;
        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;

            _associationController.OnRoundChange += ChangeImage;
        }

        private void ChangeImage(int i)
        {
            if(i >= Images.Length)

            _image.sprite = Images[i];
        }
    }
}