using System;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Feedback.Turning
{
    public class TurningDiskGraphic : TurningFeedback
    {
#if HAS_TURNING
        protected override void OnStart()
        {
            base.OnStart();
        }
        protected override void FillDisks()
        {
            for (int i = 0; i < discs.Count; i++)
            {
                discs[i].fillAmount = valuesNormalized[i];
                discs[i].transform.localEulerAngles = new Vector3(0, 0, -360 * valueSum);
                valueSum += valuesNormalized[i];
            }
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
                var temp = Instantiate(Resources.Load<GameObject>(resources.Prefabs.Find(x => x.category == PrefabCategory.UIStructure && x.id == 0).name));
                temp.transform.parent = transform;
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localScale = Vector3.one;
                temp.transform.localEulerAngles = Vector3.zero;

                discs.Add(temp.GetComponent<Image>());
                discs.Last().color = (Bygroup) ? (Color)groupsInfo.Groups.Find(x => x.Id == voteData.TurningGroup[i].GroupIndex).Color : Color.white / (i / n);
            }
        }
#endif
    }
}