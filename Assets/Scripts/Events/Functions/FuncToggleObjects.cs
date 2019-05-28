using UnityEngine;

namespace InterativaSystem.Views.Events.Functions
{
    public class FuncToggleObjects : GenericEvent
    {
        [Space(10f)]
        public GameObject[] objects;

        protected override void RunEvent()
        {
            ToggleObjects();
        }

        void ToggleObjects()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(!objects[i].activeSelf);
            }
        }
    }
}