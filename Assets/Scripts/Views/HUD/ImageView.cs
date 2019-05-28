using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD
{
    [RequireComponent(typeof(Image))]
    public class ImageView : GenericView
    {
        protected Image _image;

        protected override void OnAwake()
        {
            _image = GetComponent<Image>();
        }
    }
}