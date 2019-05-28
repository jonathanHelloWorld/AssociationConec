using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD.CheckIn
{
    public class CheckInShowOnlyFilterInput : InputView
    {
        public Transform ListRoot;

        private List<CheckInInfoView> _users;

        private RegisterController _registration;
        private ResourcesDataBase _resources;

        protected override void OnStart()
        {
            base.OnStart();

            _registration = _bootstrap.GetController(ControllerTypes.Register) as RegisterController;
            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            _users = new List<CheckInInfoView>();
            
#if HAS_SERVER
            _registration.OnRegistryReceiveEnded += GenerateUsersList;
#else
            GenerateUsersList();
#endif
        }

        void GenerateUsersList()
        {
            ClearLines();

            for (int i = 0, n = _registration.GetRegistryCount(); i < n; i++)
            {
                InstantiateUserUI();

                _users.Last().Name = _registration.GetRegistryValue(i, "Name");

                if (string.IsNullOrEmpty(_users.Last().Name))
                    _users.Last().Name = "";

                _users.Last().Index = i;

                _users.Last().Initialize();
                _users.Last().UpdataInfo();
            }
        }

        void ClearLines()
        {
            for (int i = 0, n = _users.Count; i < n; i++)
            {
                Destroy(_users[i].gameObject);
            }

            _users = new List<CheckInInfoView>();
        }

        void InstantiateUserUI()
        {
            var temp = Instantiate(Resources.Load<GameObject>(_resources.Prefabs.Find(x => x.category == PrefabCategory.UICheckInLineText).name));
            temp.transform.parent = ListRoot;

            _users.Add(temp.GetComponent<CheckInInfoView>());
        }

        protected override void ValueChanged(string value)
        {
            base.ValueChanged(value);

            Filter(value);
        }

        protected override void EndEdit(string value)
        {
            base.EndEdit(value);

            Filter(value);
        }

        void Filter(string value)
        {
            for (int i = 0, n = _users.Count; i < n; i++)
            {
                if (CheckStringInString(_users[i].Name, value))
                {
                    _users[i].Show();
                }
                else
                {
                    _users[i].Hide();
                }
            }
        }

        bool CheckStringInString(string full, string part)
        {
            return full.Contains(part);
        }
    }
}