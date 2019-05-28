using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Register
{
    public class RegisterInput : InputView
    {
        protected RegisterController _registerController;
        public Text Label;
        [Space]
        public string DataName;
        public CheckType CheckType;
        public bool IsFirstSelected;

        [Space]
        public bool IsUnique;

        [HideInInspector]
        public bool _isOk;

        private bool _isReseting;

        public GameObject CheckFeddback;
        private int _id;

        protected override void OnStart()
        {
            _registerController = _controller as RegisterController;
            _registerController.OnSubmit += Submit;

            CheckFeddback.SetActive(CheckType != CheckType.Generic && !_isOk);

            _registerController.FieldsBools.Add(_isOk);
            _id = _registerController.FieldsBools.Count - 1;

            _registerController.Reset += Reset;

            if (IsFirstSelected)
            {
                _registerController.ActiveField = this;
            }

			_timeController.RegisterInJson += AddNewValues; 

            //input.onEndEdit.AddListener(delegate { _registerController.CloseKeyboard(); });
            //input.OnSelect.AddListener(delegate { _registerController.CloseKeyboard(); });
        }

        void Reset()
        {
            if (input == null) return;

            _isReseting = true;
            input.text = "";
            ChackString(input.text);
            _registerController.FieldsBools[_id] = _isOk;

            if (IsFirstSelected)
            {
                _registerController.ActiveField = this;
            }
            _isReseting = false;
        }

        void ChackString(string value)
        {
            if (IsUnique)
            {
                string reg;
                if (_registerController.TryGetRegistryValue(DataName, out reg))
                {
                    switch (CheckType)
                    {
                        case CheckType.Generic:
                            _isOk = !string.IsNullOrEmpty(value) && reg != null && reg != value ||
                                !string.IsNullOrEmpty(value) && reg == null;
                            break;
                        case CheckType.CPF:
                            _isOk = Utils.CpfValid(value) && reg != null && reg != value ||
                                !string.IsNullOrEmpty(value) && reg == null;
                            break;
                        case CheckType.Mail:
                            _isOk = Utils.EmailIsValid(value) && reg != null && reg != value ||
                                !string.IsNullOrEmpty(value) && reg == null;
                            break;
                    }
                    return;
                }
            }

            switch (CheckType)
            {
                case CheckType.Generic:
                    _isOk = !string.IsNullOrEmpty(value);
                    break;
                case CheckType.CPF:
                    _isOk = Utils.CpfValid(value);
                    break;
                case CheckType.Mail:
                    _isOk = Utils.EmailIsValid(value);
                    break;
            }
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (_isReseting) return;
            base.OnSelect(eventData);

            if(_bootstrap.IsMobile) return;

            _registerController.ShowKeyboard();
            _registerController.ActiveField = this;
        }

        protected override void ValueChanged(string value)
        {
            if (_isReseting) return;

            ChackString(value);
            _registerController.FieldsBools[_id] = _isOk;
        }
        public void AddValues()
        {
            if (_isReseting) return;

            if (_isOk)
                _registerController.AddRegisterValue(DataName, input.text, true);

            if (CheckFeddback != null)
            {
                CheckFeddback.SetActive(!_isOk);
            }
        }

		public void AddNewValues(string DataName,string Value) 
		{

			_registerController.AddRegisterValue(DataName, Value, true);

		}

        protected override void EndEdit(string value)
        {
            if (_isReseting) return;

            if (_isOk)
                _registerController.AddRegisterValue(DataName, value, true);

            if (CheckFeddback != null)
            {
                CheckFeddback.SetActive(!_isOk);
            }
        }
        void Submit()
        {
            if (_isReseting) return;
        }
    }
}