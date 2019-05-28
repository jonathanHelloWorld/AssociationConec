using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InterativaSystem.Views.Events
{
    [RequireComponent(typeof(Button))]
    public class ExecuteEventOnClick : ExecuteEvent
    {
        Button _bt;

        protected override void OnStart()
        {
            base.OnStart();

            _bt = GetComponent<Button>();
            _bt.onClick.AddListener(OnClick);
        }
        protected virtual void OnClick()
        {
            EventStart();
        }
    }
}
