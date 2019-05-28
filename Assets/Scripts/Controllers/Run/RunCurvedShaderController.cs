using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InterativaSystem.Controllers.Run
{
    [AddComponentMenu("ModularSystem/Controllers/Run/Curve Shader Controller")]
    public class RunCurvedShaderController : GenericController
    {

        private RunController _runController;

        private List<Material> _curvedShaders;
        public Shader CurvedShader;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.CurveShader;
        }


        protected override void OnStart()
        {
            _runController = _bootstrap.GetController(ControllerTypes.Run) as RunController;
            _runController.OnGameStart += GetSaders;
            
        }

        private void GetSaders()
        {
            var objs = GameObject.FindObjectsOfType<Renderer>().ToList();
            var renderers = objs.FindAll(x => x.material.shader == CurvedShader);

            _curvedShaders = new List<Material>();
            for (int i = 0; i < renderers.Count; i++)
            {
                _curvedShaders.Add(renderers[i].material);
            }

            for (int i = 0, n = _curvedShaders.Count; i < n; i++)
            {
                _curvedShaders[i].SetFloat("_CurvatureX", _runController.Curve);
            }
        }

        public void Update()
        {
            if (_runController == null || !_runController.IsGameRunning) return;

            for (int i = 0, n = _curvedShaders.Count; i < n; i++)
            {
                _curvedShaders[i].SetFloat("_CurvatureX", _runController.Curve);
            }
        }
    }
}