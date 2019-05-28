using System.Collections;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    public class MosaicController : GenericController
    {
        public int Seed;
        public RectTransform Root, IniRoot;
        public Transform HighlightPos, OutPos;
        private TimeController _timeController;

        private MosaicData _mosaicData;
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Mosaic;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                LoadDatas();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                CallAction(10);
            }
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                _timeController.AddToTimeScale(-0.2f);
            }
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                _timeController.AddToTimeScale(0.2f);
            }
        }

        public IEnumerator UpdateRot()
        {
            yield return new WaitForSeconds(60);

            while (true)
            {
                yield return new WaitForSeconds(5);

                LoadDatas();
            }
        }

        public void LoadDatas()
        {
            _mosaicData.LoadDatas();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _mosaicData = _bootstrap.GetModel(ModelTypes.Mosaic) as MosaicData;
            _timeController = _bootstrap.GetController(ControllerTypes.Time) as TimeController;

            CallGenericAction += StartLoadingDatas;
        }

        private void StartLoadingDatas(int value)
        {
            if(value == 5)
                StartCoroutine(UpdateRot());
        }
    }
}