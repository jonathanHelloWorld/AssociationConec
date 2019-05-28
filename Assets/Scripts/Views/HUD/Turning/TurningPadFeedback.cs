using System;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Turning
{
    public class TurningPadFeedback : GenericView
    {

        private TurningVoteData voteData;
        private ResourcesDataBase resources;
        public List<TurningPadFeedbackObject> pads;

        private List<Action> stack;

        protected override void OnStart()
        {
            base.OnStart();

            voteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
            resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;
            voteData.VoteReceived += UpdateData;

            stack = new List<Action>();
        }

        void UpdateData()
        {
            for (int i = 0, n = voteData.PadsInfo.Count; i < n; i++)
            {
                if (pads.Exists(x => x.PadId == voteData.PadsInfo[i].PadId))
                {
                    //PadsInfo[PadsInfo.FindIndex(x => x.PadId == pad.PadId)] = pad;
                }
                else
                {
                    //stack.Add(() => Temp());
                    var i1 = i;
                    stack.Add(()=> InstantiatePad(voteData.PadsInfo[i1].PadId));
                }
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            CallStack();
        }

        void CallStack()
        {
            if (stack.Count <= 0) return;

            for (int i = 0, n = stack.Count; i < n; i++)
            {
                stack[i].Invoke();
            }

            stack = new List<Action>();
        }

        void InstantiatePad(string id)
        {
            var temp = Instantiate(Resources.Load<GameObject>(resources.Prefabs.Find(x => x.category == PrefabCategory.UITurningLine).name));

            temp.transform.parent = transform;
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localEulerAngles = Vector3.zero;
            temp.transform.localScale = Vector3.one;

            pads.Add(temp.GetComponent<TurningPadFeedbackObject>());

            pads.Last().PadId = id;
            pads.Last().Initialize();
        }
    }
}