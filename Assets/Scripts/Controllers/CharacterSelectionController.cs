using InterativaSystem.Models;
using UnityEngine;
using CharacterInfo = InterativaSystem.Models.CharacterInfo;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Character Selection Controller")]
    public class CharacterSelectionController : GenericController
    {
        protected CharacterSelectionData _characterSelection;

        public event SimpleEvent OnCharacterUpdated, OnCharactersReceiveEnded;

        #region Initialization
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Characters;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            _characterSelection = _bootstrap.GetModel(ModelTypes.Characters) as CharacterSelectionData;
            _characterSelection.OnValueUpdated += OnCharacterUpdatedEvent;
            _characterSelection.OnNewValue += OnCharacterUpdatedEvent;
            _characterSelection.OnCharactersReceiveEnded += OnCharactersReceiveEndedEvent;
        }

        private void OnCharacterUpdatedEvent(CharacterInfo ch)
        {
            if (OnCharacterUpdated != null) OnCharacterUpdated();
        }
        private void OnCharacterUpdatedEvent()
        {
            if (OnCharacterUpdated != null) OnCharacterUpdated();
        }

        void OnCharactersReceiveEndedEvent()
        {
            if (OnCharactersReceiveEnded != null) OnCharactersReceiveEnded();
        }

        #endregion

        #region Get Setters
        public bool TryGetCharacter(string uid, out CharacterInfo ch)
        {
            ch = null;
            if (_characterSelection.TryGetSelectedCharacter(uid, out ch))
            {
                return true;
            }
            return false;
        }

        public CharacterInfo GetSelectedCharacter()
        {
            return _characterSelection.GetSelectedCharacter();
        }
        public CharacterInfo GetSelectedCharacter(string uid)
        {
            return _characterSelection.GetSelectedCharacter(uid);
        }

        public void SetIdValue(int id)
        {
            _characterSelection.SetIdValue(id);
            if (OnCharacterUpdated != null) OnCharacterUpdated();
        }
        public void SetIdValue(int id, PrefabCategory cat)
        {
            _characterSelection.SetValues(id, id, cat);
            if (OnCharacterUpdated != null) OnCharacterUpdated();
        }

        #endregion

    }
}