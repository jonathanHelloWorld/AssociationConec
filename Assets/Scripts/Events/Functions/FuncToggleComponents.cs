using UnityEngine;

namespace InterativaSystem.Views.Events
{
    public class FuncToggleComponents : GenericEvent
    {
        [Space(10f)]
        public Behaviour[] components;

        protected override void RunEvent()
        {
            ToggleComponents();
        }

        void ToggleComponents()
        {
            for (int i = 0; i < components.Length; i++)
            {
                components[i].enabled = !components[i].enabled;
            }
        }
    }
}
