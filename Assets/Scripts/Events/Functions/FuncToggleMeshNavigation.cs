using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InterativaSystem.Views.Events
{
    public class FuncToggleMeshNavigation : GenericEvent
    {
        [Space(10f)]
        public UnityEngine.AI.NavMeshAgent[] agents;
        public bool toggle;

        protected override void RunEvent()
        {
            ToggleAgents();
        }

        void ToggleAgents()
        {
            for (int i = 0; i < agents.Length; i++)
            {
                agents[i].enabled = toggle;
            }
        }
    }
}
