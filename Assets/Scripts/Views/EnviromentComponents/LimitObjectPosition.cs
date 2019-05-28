using DG.Tweening;
using UnityEngine;

namespace InterativaSystem.Views.EnviromentComponents
{
    public class LimitObjectPosition : GenericView
    {
        public Transform minLimit, maxLimit;
        public bool limitX, limitY, limitZ;

        private Vector3 lastPosition, iniPosition;

        protected override void OnStart()
        {
            base.OnAwake();

            iniPosition = transform.position;
            lastPosition = transform.position;
        }

        protected override void OnLateUpdate()
        {
            base.OnUpdate();

            if (transform.position.x < minLimit.position.x ||
                transform.position.y < minLimit.position.y ||
                transform.position.z < minLimit.position.z
                )
            {
                transform.position = lastPosition;
            }
            if (transform.position.x > maxLimit.position.x ||
                transform.position.y > maxLimit.position.y ||
                transform.position.z > maxLimit.position.z
                )
            {
                transform.position = lastPosition;
            }
            /**/
            if (limitX)
            {
                transform.position = new Vector3(iniPosition.x, transform.position.y, transform.position.z);
            }
            if (limitY)
            {
                transform.position = new Vector3(transform.position.x, iniPosition.y, transform.position.z);
            }
            if (limitZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, iniPosition.z);
            }

            lastPosition = transform.position;
        }
    }
}