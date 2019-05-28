using System.Linq;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.Instantiator
{
    public class InstatiatorUsingChildsPosition : InstantiatorView
    {
        private Transform[] _childsTransform;
        private ResourcesDataBase _resources;
        private bool _generate;
        public float Delay;
        private float _passedTime;
        
        protected override void OnStart()
        {
            base.OnStart();

            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            GetChilds();

            _controller.OnGameStart += StartGeneration;
            _controller.OnGameEnd += StopGeneration;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!_generate) return;

            if (_passedTime >= Delay)
            {
                _passedTime = 0;
                Generate();
            }
            _passedTime += Time.deltaTime;
        }

        private void StopGeneration()
        {
            _generate = false;
        }

        private void StartGeneration()
        {
            _generate = true;
        }

        void GetChilds()
        {
            var n = transform.childCount;
            _childsTransform = new Transform[n];
            for (int i = 0; i < n; i++)
            {
                _childsTransform[i] = transform.GetChild(i);
            }
        }

        void Generate()
        {
            Generate(1);
        }

        void Generate(int qnt)
        {
            var rnd = new System.Random();
            var max = _resources.Prefabs.FindAll(x => x.category == Category).Count;
            if (max == 0) return;
            for (int i = 0; i < qnt; i++)
            {
                var next = (float)rnd.NextDouble();
                var temp = Instantiate(Resources.Load<GameObject>(_resources.GetPrefabByProbability(next, Category).name));
                var scl = temp.transform.localScale;
                var child = rnd.Next(0, _childsTransform.Length);

                temp.transform.parent = _childsTransform[child];
                temp.transform.localPosition= Vector3.zero;
                temp.transform.localScale = scl;

                temp.transform.parent = transform.parent;
                temp.GetComponent<GenericView>().Initialize();
            }
        }
    }
}