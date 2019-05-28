using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageView : GenericView
    {
        protected RawImage _image;
        protected override void OnAwake()
        {
            base.OnAwake();

            _image = GetComponent<RawImage>();
        }
    }
}