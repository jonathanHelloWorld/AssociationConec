using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents.Input
{
    public class ScaleByInputFloat : GenericView
    {
        protected InputController _inputController;
        public int FloatId;
        public bool UseInitialAsMinimun = true;
        public Vector3 AxisToScale;
        private Vector3 _iniScale;

        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _controller as InputController;

            _iniScale = transform.localScale;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_inputController.FloatControllers.Count < FloatId) return;

            transform.localScale = new Vector3(
                _iniScale.x * (AxisToScale.x < 0.01f ? 1 : (UseInitialAsMinimun ? 1 + _inputController.FloatControllers[FloatId] : _inputController.FloatControllers[FloatId]) * AxisToScale.x),
                _iniScale.y * (AxisToScale.y < 0.01f ? 1 : (UseInitialAsMinimun ? 1 + _inputController.FloatControllers[FloatId] : _inputController.FloatControllers[FloatId]) * AxisToScale.y),
                _iniScale.z * (AxisToScale.z < 0.01f ? 1 : (UseInitialAsMinimun ? 1 + _inputController.FloatControllers[FloatId] : _inputController.FloatControllers[FloatId]) * AxisToScale.z)
                );
        }
    }
}