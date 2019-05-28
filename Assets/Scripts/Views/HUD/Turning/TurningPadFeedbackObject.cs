using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Turning
{
    public class TurningPadFeedbackObject : GenericView
    {
        [HideInInspector]
        public string PadId;

        public Text Id;
        public Text Vote;
        public Text Time;

        private TurningVoteData voteData;

        protected override void OnStart()
        {
            base.OnStart();

            voteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;

            Id.text = PadId;
            Vote.text = "";
            Time.text = "";
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (voteData.PadsInfo.Exists(x => x.PadId == PadId))
            {
                var pad = voteData.PadsInfo.Find(x => x.PadId == PadId);
                Vote.text = pad.Vote;
                Time.text = pad.Time.ToString();
            }
            else
            {
                Vote.text = "";
                Time.text = "";
            }
        }
    }
}