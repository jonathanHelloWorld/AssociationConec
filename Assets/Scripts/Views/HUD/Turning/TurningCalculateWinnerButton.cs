using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Turning
{
    public class TurningCalculateWinnerButton : ButtonView
    {
        private TurningVoteData voteData;

        protected override void OnStart()
        {
            base.OnStart();

            voteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
        }

        protected override void OnClick()
        {
            base.OnClick();

            voteData.GetWinnerGroup();
        }
    }
}