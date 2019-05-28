using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleView : GenericView
    {
        protected Toggle _toggle;

        protected override void OnStart()
        {
            base.OnStart();

            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(Toggled);
        }

        protected  virtual void Toggled(bool arg0) { }
    }
}