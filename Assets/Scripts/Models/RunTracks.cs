using System.Collections.Generic;
using InterativaSystem.Views.EnviromentComponents;
using UnityEngine;

namespace InterativaSystem.Models
{
    [AddComponentMenu("ModularSystem/InfoModel/ Run Tracks")]
    public class RunTracks : GenericModel
    {
        [HideInInspector]
        public List<RunTrack> Tracks;

        public float NearPointDistance = 2.5f;

        #region MonoBehaviour Methods
        void Awake()
        {
            //Mandatory set for every model
            Type = ModelTypes.Tracks;

            Tracks = new List<RunTrack>();
        }
        #endregion

        public void AddTrack(RunTrack track)
        {
            DebugLog("AddTrack");
            Tracks.Add(track);

            Tracks.Sort((a, b) => a.Id.CompareTo(b.Id));
        }
    }
}