using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Controllers.Sound
{
    [AddComponentMenu("ModularSystem/Controllers/BGM Controller")]
    public class BGMController : SoundController
    {
        public bool AutoPlay = true;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.SoundBGM;
        }
        protected override void OnStart()
        {
            base.OnStart();
            _soundPropierties = _sounds.SoundPropierties.FindAll(x => x.category == SoundCategory.BGM);

            if (AutoPlay && _soundPropierties.Count>0)
                PlaySound(_soundPropierties[0]);
        }
    }
}