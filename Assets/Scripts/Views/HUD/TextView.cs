using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Text))]
    public class TextView : GenericView
    {
        protected Text _tx;

        protected override void OnAwake()
        {
            _tx = GetComponent<Text>();
        }
    }
}