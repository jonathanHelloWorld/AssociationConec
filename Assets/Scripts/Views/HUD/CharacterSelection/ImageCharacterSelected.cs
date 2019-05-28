using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.CharacterSelection
{
    public class ImageCharacterSelected  : ImageView
    {
        public Sprite[] Characters;

        private CharacterSelectionController _character;

        protected override void OnStart()
        {
            base.OnStart();

            _character = _controller as CharacterSelectionController;

            _character.OnCharacterUpdated += UpdateCharacter;
        }

        private void UpdateCharacter()
        {
            var ch = _character.GetSelectedCharacter();

            if (ch.id < Characters.Length)
            {
                _image.sprite = Characters[ch.id];
            }
            else
            {
                _image.sprite = null;
            }
        }
    }
}