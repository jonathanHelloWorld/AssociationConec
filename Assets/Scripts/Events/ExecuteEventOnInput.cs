using InterativaSystem.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace InterativaSystem.Views.Events
{
    public class ExecuteEventOnInput : ExecuteEvent
    {
        InputController _inputController;
        PagesController _pagesController;

        public ControlerButton inputType = ControlerButton.Up;
        public string executeOnPages;

        List<int> onPages;

        protected override void OnStart()
        {
            base.OnStart();

            _inputController = _bootstrap.GetController(ControllerTypes.Input) as InputController;
            _pagesController = _bootstrap.GetController(ControllerTypes.Page) as PagesController;

            if(!string.IsNullOrEmpty(executeOnPages))
                onPages = executeOnPages.Split(',').Select(x => int.Parse(x)).ToList();

#if HAS_CONTROLLERINPUT
            _inputController.OnJoystickPrees += EventStart;
#else
            Debug.LogError("Tag HAS_CONTROLLERINPUT not set.");
#endif
        }

        void EventStart(ControlerButton btn, int index)
        {
            if (btn != inputType) return;

            EventStart();
        }

        protected override void EventStart()
        {
            if (!_bootstrap.IsAppRunning) return;

            if (onPages != null)
                if (onPages.Contains(_pagesController.actualPage))
                {
                    base.EventStart();
                    return;
                }
                    
            base.EventStart();
        }
    }
}
