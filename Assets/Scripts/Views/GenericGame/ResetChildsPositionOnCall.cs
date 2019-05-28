using UnityEngine;

namespace InterativaSystem.Views.GenericGame
{
    public class ResetChildsPositionOnCall : GenericView
    {
        public int EnableId, ResetId;

        [HideInInspector]
        public Vector3[] childPos, childRot;

        protected override void OnAwake()
        {
            base.OnAwake();

            childPos = new Vector3[transform.childCount];
            childRot = new Vector3[transform.childCount];

            for (int i = 0; i < childRot.Length; i++)
            {
                childPos[i] = transform.GetChild(i).localPosition;
                childRot[i] = transform.GetChild(i).localEulerAngles;
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            _controller.CallGenericAction += CheckShow;
        }
        private void CheckShow(int value)
        {
            if (EnableId == value)
            {
                EnableChilds();
            }
            if (ResetId == value)
            {
                ResetChilds();
            }
        }

        void EnableChilds()
        {
            for (int i = 0; i < childRot.Length; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        void ResetChilds()
        {
            for (int i = 0; i < childRot.Length; i++)
            {
                transform.GetChild(i).localPosition = childPos[i];
                transform.GetChild(i).localEulerAngles = childRot[i];
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}