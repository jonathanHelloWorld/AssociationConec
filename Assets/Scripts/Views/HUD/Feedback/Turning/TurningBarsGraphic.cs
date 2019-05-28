using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Feedback.Turning
{
    public class TurningBarsGraphic : TurningFeedback
    {
#if HAS_TURNING
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

        }
        
        public override void UpdateValues()
        {
            base.UpdateValues();
        }

        protected override void InstantiateDisks()
        {
            base.InstantiateDisks();

            for (int i = 0, n = values.Count; i < n; i++)
            {
                var temp = Instantiate(Resources.Load<GameObject>(resources.Prefabs.Find(x => x.category == PrefabCategory.UIStructure && x.id == 1).name));
                temp.transform.parent = transform;
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localScale = Vector3.one;
                temp.transform.localEulerAngles = Vector3.zero;

                discs.Add(temp.GetComponent<Image>());
                if (Colorize)
                    discs.Last().color = (Bygroup) ? (Color) groupsInfo.Groups[i].Color : DefaultColor;
                else
                    discs.Last().color = DefaultColor;
            }
        }
#endif
    }
}