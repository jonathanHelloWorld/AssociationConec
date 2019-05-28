using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Interfaces;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Feedback.Turning
{
    public class TurningFeedback : GenericView
    {
        [HideInInspector]
        public List<float> values;
        [HideInInspector]
        public List<float> valuesNormalized;
        [HideInInspector]
        public List<Image> discs;

        public bool Bygroup;
        public bool Colorize;
        public Color DefaultColor = Color.red;
        protected GroupsInfo groupsInfo;

        protected ResourcesDataBase resources;
        protected QuizTurningController quizTurning;

        protected IController ctrl;
        protected float valueSum = 0;

#if HAS_TURNING
        protected TurningVoteData voteData;
#endif

        protected override void OnStart()
        {
            base.OnStart();

            values = new List<float>();
            valuesNormalized = new List<float>();

            resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;

            quizTurning = _bootstrap.GetController(ControllerTypes.Quiz) as QuizTurningController;

#if HAS_TURNING
            voteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
            voteData.VoteUpdated += UpdateValues;
#endif

            ctrl = _bootstrap.GetController(_controllerType);
            ctrl.OnGameEnd += UpdateValues;


            discs = new List<Image>();
        }
        protected override void OnUpdate()
        {
            base.OnUpdate();

            valueSum = 0;
            Normalize();
            FillDisks();
        }

        protected virtual void FillDisks()
        {
            for (int i = 0; i < discs.Count; i++)
            {
                discs[i].fillAmount = valuesNormalized[i];
                valueSum += valuesNormalized[i];
            }
        }

        public virtual void UpdateValues()
        {
            CleanDisks();

            values = new List<float>();

            #region By Group
            if (Bygroup)
            {
#if HAS_TURNING
                //Get only one data by GroupIndex
                var distinctValues =
                    from cust in voteData.TurningGroup
                    group cust by cust.GroupIndex
                    into gcust
                    select gcust.First();

                for (int i = 0; i < distinctValues.Count(); i++)
                {
                    var votes = voteData.TurningGroup.FindAll(x => x.GroupIndex == groupsInfo.Groups[i].Id);
                    //old method => var votes = voteData.TurningGroup.FindAll(x => x.GroupIndex == i);

                    float sum = 0;
                    for (int j = 0; j < votes.Count; j++)
                    {
                        if (!voteData.PadsInfo.Exists(x => x.PadId == votes[j].PadId))
                            continue;
                        if (quizTurning.CheckIfVoteIsRight(voteData.PadsInfo.Find(x => x.PadId == votes[j].PadId).Vote))
                        {
                            sum += 1;
                            //sum += (quizTurning.CheckIfVoteIsRight(voteData.PadsInfo.Find(x => x.PadId == votes[j].PadId).Vote))? 1: 0;
                        }
                    }
                    values.Add(sum);
                }
#endif

                if (values.Count == 0)
                    return;
                InstantiateDisks();
            }
            #endregion
            #region Else
            else
            {
#if HAS_TURNING
                //Get only one data by GroupIndex
                var alts = quizTurning.AlternativesCount();

                for (int i = 0; i < alts; i++)
                {
                    var votes = voteData.PadsInfo.FindAll(x => x.Vote == (i + 1).ToString());
                    values.Add(votes.Count);
                }
#endif
                if (values.Count == 0)
                    return;
                InstantiateDisks();

            }
            #endregion

            valuesNormalized = new List<float>(values);
        }

        protected virtual void InstantiateDisks() { }

        protected virtual void Normalize()
        {
            var sum = 0f;

            for (int i = 0; i < values.Count; i++)
                sum += values[i];

            if (sum <= 0)
                return;

            for (int i = 0; i < values.Count; i++)
            {
                if (sum <= 0)
                    valuesNormalized[i] = 0;
                else
                    valuesNormalized[i] = values[i] / sum;
            }
        }
        protected void CleanDisks()
        {
            for (int i = 0, n = discs.Count; i < n; i++) 
            {
                Destroy(discs[i].gameObject);
            }
            discs = new List<Image>();
        }

#if HAS_SERVER
        public void NetworkUpdateFeedback()
        {
            UpdateValues();
        }
#endif
    }
}