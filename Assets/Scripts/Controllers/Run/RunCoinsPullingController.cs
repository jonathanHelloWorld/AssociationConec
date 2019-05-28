using System;
using System.Collections.Generic;
using InterativaSystem.Models;
using InterativaSystem.Views;
using UnityEngine;

namespace InterativaSystem.Controllers.Run
{
    [AddComponentMenu("ModularSystem/Controllers/Run/Coins Pooling Controller")]
    public class RunCoinsPullingController : RunPullingController
    {
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.CoinPulling;
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
            var coins = _resources.Prefabs.FindAll(x => x.category == PrefabCategory.Coin);
            var rnd = new System.Random(DateTime.UtcNow.Millisecond);
            var tracks = _tracksInfo.Tracks;

            if (coins.Count == 0)
            {
                Debug.LogError("Not Fount in te category Coin");
                return;
            }

            //create the needed coins for the pooling
            for (int i = 0; i < _controller.PoolingDistance * Amount; i++)
            {
                //instantiate a random coin
                var temp = Instantiate(Resources.Load<GameObject>(coins[rnd.Next(0, coins.Count)].name));


                var refPos = rnd.Next(0, tracks.Count);

                while (ExcludeTracks != null && ExcludeTracks.Count > 0 && ExcludeTracks.Contains(refPos) && ExcludeTracks.Count < tracks.Count)
                {
                    refPos = rnd.Next(0, tracks.Count);
                }

                if (tracks.Count > 0)
                    temp.transform.position = tracks[refPos].GetPointOnTrack(-100);

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