using System;
using System.Collections.Generic;
using InterativaSystem.Models;
using InterativaSystem.Views;
using InterativaSystem.Views.EnviromentComponents;
using UnityEngine;

namespace InterativaSystem.Controllers.Run
{
    [AddComponentMenu("ModularSystem/Controllers/Run/Obstacles Pooling Controller")]
    public class RunObstaclesPullingController : RunPullingController
    {
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.ObstaclePulling;
        }

        protected override void InstantiateObjects()
        {
            //Clean the list every time that instantiates
            if (_objects != null && _objects.Count > 0)
            {
                for (int i = 0, n = _objects.Count; i < n; i++)
                {
                    Destroy(_objects[i]);
                }
            }
            _objects = new List<GameObject>();

            //Find all Coins in the database
            var obstables = _resources.Prefabs.FindAll(x => x.category == PrefabCategory.Obstacle);
            var rnd = new System.Random(DateTime.UtcNow.Millisecond);
            var tracks = _tracksInfo.Tracks;

            if (obstables.Count == 0)
            {
                Debug.LogError("Not Fount in the category Obstacle");
                return;
            }

            //create the needed coins for the pooling
            for (int i = 0; i < _controller.PoolingDistance * Amount; i++)
            {
                //instantiate a random coin
                var temp = Instantiate(Resources.Load<GameObject>(obstables[rnd.Next(0, obstables.Count)].name));

                //position on the first track if exist one
                if (tracks.Count > 0)
                    temp.transform.position = tracks[0].GetPointOnTrack(-100);

                //add the prefab to the list
                _objects.Add(temp);

                try
                {
                    temp.GetComponent<GenericView>().Initialize();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            CallOnInstantiated();
        }
    }
}