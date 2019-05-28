using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Views.Events.Functions
{
    public class FuncEndGame : GenericEvent
    {
        protected override void RunEvent()
        {
            base.RunEvent();

            _bootstrap.EndGame(_controller);
        }
    }
}
