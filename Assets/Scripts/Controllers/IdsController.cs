using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/Ids Controller")]
    public class IdsController : GenericController
    {
        private IdsData _idsData;

#if HAS_SERVER
        public event GenericController.SimpleEvent OnIdsReceiveEnded;
        public event GenericController.SimpleEvent OnSessionUpdate;
#endif

        #region Initialization
        void Awake()
        {
            Type = ControllerTypes.Id;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void GetReferences()
        {
            base.GetReferences();

            _idsData = _bootstrap.GetModel(ModelTypes.Ids) as IdsData;
#if HAS_SERVER
            _idsData.OnIdsReceiveEnded += OnIdsReceiveEndedEvent;
            _idsData.OnSessionUpdate += OnSessionUpdateEvent;
#endif
        }

        void OnIdsReceiveEndedEvent()
        {
#if HAS_SERVER
            if (OnIdsReceiveEnded != null) OnIdsReceiveEnded();
#endif
        }

        void OnSessionUpdateEvent()
        {
#if HAS_SERVER
            if (OnSessionUpdate != null) OnSessionUpdate();
#endif
        }
        #endregion

        #region Get Setters
        //Get
        public string GetActualId()
        {
            return _idsData.GetActual().uid;
        }
        public Id GetActual()
        {
            return _idsData.GetActual();
        }

        //Set
        public void CreateNewSession()
        {
            _idsData.NewSession();
        }
#endregion

        public string GetActualSession()
        {
            return _idsData.GetActualSession();
        }

        public void CreateNewId()
        {
            _idsData.AddNewValue();
        }
    }
}