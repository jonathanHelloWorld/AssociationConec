using System.Collections.Generic;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class RunTrack : GenericView
    {
        public int Id;
        float _nearPointDistance;

        private Vector3 _initialPosition, _direction;

        private List<List<Vector3>> _objectsOnTrack;
        private RunTracks TrackInfo;

        private Ray TrackRay;

        protected override void OnStart()
        {
            _initialPosition = transform.position;
            _direction = transform.TransformDirection(Vector3.forward);

            TrackRay = new Ray(_initialPosition, _direction);

            AddTrackToModel();

            ClearObjects();

            _nearPointDistance = TrackInfo.NearPointDistance;

            _controller.Reset += Resetrack;
        }

        private void Resetrack()
        {
            ClearObjects();
        }

        void AddTrackToModel()
        {
            TrackInfo = _bootstrap.GetModel(ModelTypes.Tracks) as RunTracks;

            TrackInfo.AddTrack(this);
        }

        public Vector3 GetPointOnTrack(float distance)
        {
            return TrackRay.GetPoint(distance);
        }
        public bool TryGetPointOnTrack(float distance, out Vector3 point, int trackOcclusion)
        {
            point = TrackRay.GetPoint(distance);

            var dif = (trackOcclusion + 1) - _objectsOnTrack.Count;
            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    _objectsOnTrack.Add(new List<Vector3>());
                }
            }

            if (_objectsOnTrack[trackOcclusion].Exists(p => Vector3.Distance(p, TrackRay.GetPoint(distance)) < _nearPointDistance))
            {
                return false;
            }
            
            _objectsOnTrack[trackOcclusion].Add(point);
            return true;
        }
        public bool TryGetPointOnTrack(float distance, out Vector3 point, float nearPointDistance, int trackOcclusion)
        {
            point = TrackRay.GetPoint(distance);

            var dif = (trackOcclusion + 1) - _objectsOnTrack.Count;
            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    _objectsOnTrack.Add(new List<Vector3>());
                }
            }

            if (_objectsOnTrack[trackOcclusion].Exists(p => Vector3.Distance(p, TrackRay.GetPoint(distance)) < nearPointDistance))
            {
                return false;
            }
            
            _objectsOnTrack[trackOcclusion].Add(point);
            return true;
        }

        public void ClearObjects()
        {
            _objectsOnTrack = new List<List<Vector3>>();
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 3, Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up), Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down), Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left), Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right), Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back), Color.magenta);
        }
#endif
    }
}