using InterativaSystem;
using InterativaSystem.Controllers;
using InterativaSystem.Views;
using UnityEngine;

namespace InterativaSystem.Views.Events.Functions
{
    public class FuncCallAction : GenericEvent
    {
        [Space(10f)]
        public int Id;

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void RunEvent()
        {
            _controller.CallAction(Id);
        }
    }
}