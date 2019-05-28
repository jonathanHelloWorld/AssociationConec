using System;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Models;
using InterativaSystem.Views;
using UnityEngine;

namespace InterativaSystem.Controllers.Run
{
    [AddComponentMenu("ModularSystem/Controllers/Run/Scenario Pooling Controller")]
    public class RunScenarioPullingController : RunPullingController
    {
        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.ScenarioPulling;
        }

        protected override void InstantiateObjects()
        {
            float poolinDistance;

            //Clean the list every time that instantiates
            if (_objects != null && _objects.Count > 0)
            {
                for (int i = 0, n = _objects.Count; i < n; i++)
                {
                    Destroy(_objects[i]);
                }
            }
            _objects = new List<GameObject>();

            //Find all Areas in the database
            var area = _resources.Prefabs.FindAll(x => x.category == PrefabCategory.Environment);

            var rnd = new System.Random(555);
            var tracks = _tracksInfo.Tracks;

            if (area.Count == 0)
            {
                Debug.LogError("Not Fount in the category Environment");
                return;
            }

            _areaSizes = new List<float>();
            for (int i = 0, n = area.Count; i < n; i++)
            {
                //instantiate a random area
                var temp = Instantiate(Resources.Load<GameObject>(area[i].name));


                //Calculate the entire size of the scenario
                var childs = temp.transform.GetComponentsInChildren<Renderer>();
                var bounds = new Bounds(temp.transform.position, Vector3.zero);

                if (temp.GetComponent<Renderer>() != null)
                    bounds.Encapsulate(temp.GetComponent<Renderer>().bounds);

                for (int j = 0; j < childs.Length; j++)
                {
                    var granChilds = childs[j].transform.GetComponentsInChildren<Renderer>();

                    for (int k = 0; k < granChilds.Length; k++)
                    {
                        bounds.Encapsulate(granChilds[k].bounds);
                    }

                    bounds.Encapsulate(childs[j].bounds);
                }

                //Forced to calculate only size in Z because the instantiation methods in the project
                _areaSizes.Add(bounds.size.z - 0.04f);

                if (temp.GetComponent<GenericView>() != null)
                    temp.GetComponent<GenericView>().Initialize();

                _objects.Add(temp);
            }

            float totalLength = 0;
            for (int i = 0; i < _areaSizes.Count; i++)
            {
                totalLength += _areaSizes[i];
            }

            poolinDistance = _areaSizes[0] * MinimunInTrack;

            //create the needed Areas for the pooling
            if (totalLength < poolinDistance)
            {
                var qnt = MinimunInTrack - area.Count; //(int)((poolinDistance - totalLength) / _areaSizes[0]);
                for (int i = 0; i < qnt; i++)
                {
                    //instantiate a random coin
                    var temp = Instantiate(Resources.Load<GameObject>(area[rnd.Next(0, area.Count)].name));

                    //Calculate the entire size of the scenario
                    var childs = temp.transform.GetComponentsInChildren<Renderer>();
                    var bounds = new Bounds(temp.transform.position, Vector3.zero);

                    if (temp.GetComponent<Renderer>() != null)
                        bounds.Encapsulate(temp.GetComponent<Renderer>().bounds);

                    for (int j = 0; j < childs.Length; j++)
                    {
                        var granChilds = childs[j].transform.GetComponentsInChildren<Renderer>();

                        for (int k = 0; k < granChilds.Length; k++)
                        {
                            bounds.Encapsulate(granChilds[k].bounds);
                        }
                        bounds.Encapsulate(childs[j].bounds);
                    }

                    //Forced to calculate only size in Z because the instantiation methods in the project
                    _areaSizes.Add(bounds.size.z);
                    
                    if (temp.GetComponent<GenericView>() != null)
                        temp.GetComponent<GenericView>().Initialize();

                    //position on the first track if exist one
                    if (tracks.Count > 0)
                        temp.transform.position = tracks[0].GetPointOnTrack(-1000);

                    //add the prefab to the list
                    _objects.Add(temp);
                }
            }

            CallOnInstantiated();
        }
    }
}