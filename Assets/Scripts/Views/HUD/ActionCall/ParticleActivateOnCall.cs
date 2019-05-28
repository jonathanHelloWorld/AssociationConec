using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Views.HUD.ActionCall
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleActivateOnCall : GenericView
    {

        public List<int> Ids;

        private ParticleSystem _particleSystem;

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CheckShow;
            _controller.GenericActionEnded += Hide;

            _particleSystem = GetComponent<ParticleSystem>();

            if (Ids.Contains(0))
            {
                Play();
            }
            else
            {
                Stop();
            }

        }
        private void CheckShow(int value)
        {
            var result = false;
            if (Ids != null && Ids.Count > 0)
            {
                //if (Ids.Exists(x => x != value)) return;

                result = Ids.Contains(value);
            }

            if (result)
                Play();
            else
                Stop();
        }

        private void Stop()
        {
            _particleSystem.Stop();
        }

        private void Play()
        {
            _particleSystem.Play();
        }

        private void Hide(int value)
        {
            if (Ids != null && Ids.Count > 0)
            {
                if (Ids.Exists(x => x != value)) return;
            } else return;

            _particleSystem.Stop();
        }
    }
}