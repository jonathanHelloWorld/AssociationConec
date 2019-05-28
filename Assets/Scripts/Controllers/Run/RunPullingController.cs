using System.Collections;
using System.Collections.Generic;
using InterativaSystem.Models;
using InterativaSystem.Views.EnviromentComponents;
using UnityEngine;

namespace InterativaSystem.Controllers.Run
{
    public class RunPullingController : GenericController
    {
        [HideInInspector]
        public bool StartSet;

        protected List<GameObject> _objects;
        protected RunController _controller;
        protected ResourcesDataBase _resources;
        protected RunTracks _tracksInfo;
        protected int _counter = 0;
        protected int[] _pos;
        private bool[] _onces;

        public List<int> ExcludeTracks;
        public bool DoNotObstruct;

        public event SimpleEvent OnInstantiated;

        private bool instanced;

        [Header("Settings")]
        [Range(1, 15)]
        public int MinimunInTrack = 3;

        //used for pooling objects whith seamless conections enables onle with 'CanSwitchTracks'
        protected List<float> _areaSizes;
        public bool CanSwitchTracks = true;

        [Header("Settings (Only on Start)")]
        [Range(0, 100)]
        public float Amount = 0.5f;
        public float InitialOffset = 5;

        #region MonoBehaviour Methods
        protected override void OnStart()
        {
            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;
            _tracksInfo = _bootstrap.GetModel(ModelTypes.Tracks) as RunTracks;
            _controller = _bootstrap.GetController(ControllerTypes.Run) as RunController;

            _controller.Reset += ResetPool;

            StartCoroutine(InstantiateObjectsRoutine());
        }

        IEnumerator InstantiateObjectsRoutine()
        {
            for (int i = 0; i < 10; i++)
                yield return null;
            
            InstantiateObjects();
        } 

        void Update()
        {
            if (!_bootstrap.IsAppRunning) return;

            Pooling();
        }
        #endregion

        private void ResetPool()
        {
            if (!instanced) return;

            _counter = 0;

            for (int i = 0, n = _objects.Count; i < n; i++)
            {
                _objects[i].transform.position = Vector3.one * -100;
            }

            _pos = new int[_objects.Count];
            _onces = new bool[_objects.Count];

            if (!CanSwitchTracks) return;

            var tracks = _tracksInfo.Tracks;
            var rnd = new System.Random();

            for (int j = 0, refPos = 0, m = _pos.Length; j < m; j++)
            {
                if (j % MinimunInTrack == 0)
                    refPos = rnd.Next(0, tracks.Count);

                while (ExcludeTracks != null && ExcludeTracks.Count > 0 && ExcludeTracks.Contains(refPos) && ExcludeTracks.Count < tracks.Count)
                {
                    refPos = rnd.Next(0, tracks.Count);
                }

                _pos[j] = refPos;
            }

        }

        protected void CallOnInstantiated()
        {
            instanced = true;
            if (OnInstantiated != null) OnInstantiated();
        }

        protected virtual void InstantiateObjects() { }

        void Pooling()
        {
            if (!instanced) return;

            float poolinDistance;
            float distance = 0;

            List<RunTrack> tracks;

            var rnd = new System.Random();

            if (CanSwitchTracks) //For itens and obstacles
            {
                poolinDistance = _controller.PoolingDistance;
                distance = poolinDistance / _objects.Count;
            }
            else //For Scenario
            {
                //Set the same size for all itens in the pooling based on the first
                poolinDistance = (_areaSizes[0] * MinimunInTrack);
                distance = _areaSizes[0];
            }

            //Get the possition on the game
            var traveled = _controller.DistanceTraveled + poolinDistance;

            if (!CanSwitchTracks) //For Scenario, force all tracks into one
            {
                tracks = new List<RunTrack>();
                tracks.Add(_tracksInfo.Tracks[0]);
            }
            else //For itens and obstacles
            {
                tracks = _tracksInfo.Tracks;
            }

            // Create the checkers for colisions on instantiation
            if (_pos == null)
            {
                _onces = new bool[_objects.Count];
                _pos = new int[_objects.Count];

                for (int j = 0, refPos = 0, m = _pos.Length; j < m; j++)
                {
                    if (j % MinimunInTrack == 0)
                        refPos = rnd.Next(0, tracks.Count);

                    while (ExcludeTracks != null && ExcludeTracks.Count > 0 && ExcludeTracks.Contains(refPos) && ExcludeTracks.Count < tracks.Count)
                    {
                        refPos = rnd.Next(0, tracks.Count);
                    }

                    _pos[j] = refPos;

                    _onces[j] = false;
                }
            }

            //Pooling
            for (int i = 0, n = _objects.Count; i < n; i++)
            {
                // Once get if has already been moved
                if (!_onces[i] && traveled > (poolinDistance * _counter + distance * (i+1) + InitialOffset))
                {
                    _onces[i] = true;

                    if (!Move(tracks, distance, i, poolinDistance))
                    {
                        continue;
                    }

                    if (i == n - 1)
                    {
                        for (int j = 0, refPos = 0, m = _pos.Length; j < m; j++)
                        {
                            if (j % MinimunInTrack == 0)
                                refPos = rnd.Next(0, tracks.Count);

                            while (ExcludeTracks != null && ExcludeTracks.Count > 0 && ExcludeTracks.Contains(refPos) && ExcludeTracks.Count < tracks.Count)
                            {
                                refPos = rnd.Next(0, tracks.Count);
                            }

                            _pos[j] = refPos;

                            _onces[j] = false;
                        }
                        _counter++;
                    }
                    CallResetDependencies();
                }
            }
        }

        private bool Move(List<RunTrack> tracks, float distance, int i, float poolinDistance)
        {
            Vector3 point;

            //Always go on scenarios
            if (!CanSwitchTracks)
            {
                //_objects[i].transform.position = tracks[_pos[i]].GetPointOnTrack(distance * i + poolinDistance * _counter + InitialOffset);

                if (tracks[_pos[i]].TryGetPointOnTrack(distance * i + poolinDistance * _counter + InitialOffset, out point, distance*0.8f, 1))
                {
                    _objects[i].transform.position = point;
                    return true;
                }
                return false;
                /**/
            }

            if (DoNotObstruct)
            {
                _objects[i].transform.position = tracks[_pos[i]].GetPointOnTrack(distance * i + poolinDistance * _counter + InitialOffset);
                return true;
            }

            if (tracks[_pos[i]].TryGetPointOnTrack(distance * i + poolinDistance * _counter + InitialOffset, out point, 0))
            {
                _objects[i].transform.position = point;
                return true;
            }

            for (int j = 0, m = tracks.Count; j < m; j++)
            {
                if (tracks[j].TryGetPointOnTrack(distance * i + poolinDistance * _counter + InitialOffset, out point, 0))
                {
                    _objects[i].transform.position = point;
                    return true;
                }
            }

            return false;
        }
    }
}