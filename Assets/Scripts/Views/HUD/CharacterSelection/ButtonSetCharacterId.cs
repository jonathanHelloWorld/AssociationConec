using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.CharacterSelection
{
    public class ButtonSetCharacterId : ButtonView
    {
        public int Id;
        
        public PrefabCategory prefabCategory;

        protected CharacterSelectionController _characterSelection;
        protected override void OnStart()
        {
            base.OnStart();

            _characterSelection = _controller as CharacterSelectionController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _characterSelection.SetIdValue(Id, prefabCategory);
        }
    }
}