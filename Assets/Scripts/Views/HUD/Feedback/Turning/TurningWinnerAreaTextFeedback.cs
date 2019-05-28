using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Feedback.Turning
{
    public class TurningWinnerAreaTextFeedback : TextView
    {
        private GroupsInfo groupsInfo;
        private TurningVoteData turningVote;

        protected override void OnStart()
        {
            base.OnStart();

            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
            turningVote = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;

            turningVote.VoteUpdated += UpdateWinner;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

#if HAS_SERVER
            if (groupsInfo.Groups.Exists(x => x.Id == turningVote.GetWinnerGroup()))
                _tx.text = groupsInfo.Groups.Find(x => x.Id == turningVote.GetWinnerGroup()).AreaPoints.ToString("00");
            else
                _tx.text = "";
#endif

        }

        protected void UpdateWinner()
        {
            base.OnUpdate();

            _tx.text = turningVote.GetWinnerGroup().ToString("00");
        }
    }
}