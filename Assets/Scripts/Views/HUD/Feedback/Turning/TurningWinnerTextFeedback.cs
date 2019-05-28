using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Feedback.Turning
{
    public class TurningWinnerTextFeedback : TextView
    {
        private GroupsInfo groupsInfo;
        private TurningVoteData turningVote;

        public bool LiveUpdate = true;

        protected override void OnStart()
        {
            base.OnStart();

            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
            turningVote = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;

            turningVote.VoteUpdated += UpdateWinner;

            if (_controller != null)
            {
                _controller.OnGamePrepare += UpdateWinner;
                _controller.OnGameEnd += UpdateWinner;
            }
            UpdateWinner();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (LiveUpdate)
            {
                if (groupsInfo.Groups.Exists(x => x.Id == turningVote.GetWinnerGroup()))
                    _tx.text = groupsInfo.Groups.Find(x => x.Id == turningVote.GetWinnerGroup()).Name;
                else
                    _tx.text = "";
            }

        }

        protected void UpdateWinner()
        {
            base.OnUpdate();

            if (groupsInfo.Groups.Exists(x => x.Id == turningVote.GetWinnerGroup()))
                _tx.text = groupsInfo.Groups.Find(x => x.Id == turningVote.GetWinnerGroup()).Name;
            else
                _tx.text = "";
        }
    }
}