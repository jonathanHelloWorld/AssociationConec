using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Run;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters
{
    public class RunFollowCamera : GenericView
    {
        private RunController _runController;
        private RunTracks _tracksInfo;


        [Header("Settings")]
        public float Offset = 3;

        /*
        private Vector3 _lastPosition;
        private float _velocity;
        private float _speed = 1.5f;
        /**/

        protected override void OnStart()
        {
            _runController = _controller as RunController;
            _tracksInfo = _bootstrap.GetModel(ModelTypes.Tracks) as RunTracks;

            /*
            _lastPosition = transform.position;
            _velocity = 0;
            _speed = _runController.FixedSpeed;
            /**/

            _offset = transform.TransformDirection(Vector3.forward);
            _offset = new Vector3(_offset.x, 0, _offset.z).normalized * Offset;
        }
        public float closeTolerance = 0.1f;
        private Vector3 _offset;
        protected override void OnUpdate()
        {
            var point = _tracksInfo.Tracks[0].GetPointOnTrack(_runController.DistanceTraveled);
            var destination = new Vector3(point.x, transform.position.y, point.z);
            
            destination -= _offset;

            transform.position = destination;
            /*
            var movDir = destination - _lastPosition;

            movDir = new Vector3(Mathf.Round(movDir.x * 10), Mathf.Round(movDir.y * 10), Mathf.Round(movDir.z * 10)) / 10;

            transform.position += movDir * _speed * Time.deltaTime;

            _lastPosition = transform.position;
            /**/
        }
    }
}