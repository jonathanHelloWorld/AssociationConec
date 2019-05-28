using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.HUD.CharacterSelection
{
    public class ShowCharacterSelectedOnPage : DoOnPageAuto
    {
        public GameObject[] Characters;

        private CharacterSelectionController _character;

        GameObject selected;

        protected override void OnStart()
        {
            base.OnStart();

            _character = _bootstrap.GetController(ControllerTypes.Characters) as CharacterSelectionController;
        }

        protected override void DoSomething()
        {
            var ch = _character.GetSelectedCharacter();

            if (ch.id < Characters.Length)
                Characters[ch.id].SetActive(true);
        }

        protected override void NotThisPage()
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i].SetActive(false);
            }
        }
    }
}