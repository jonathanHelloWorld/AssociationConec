using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Turning
{
    public class TurningButtonStopPadGroupRecoording : ButtonView
    {
        private TurningVoteData _turningVoteData;

        protected override void OnStart()
        {
            base.OnStart();

            _turningVoteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
        }

        protected override void OnClick()
        {
            base.OnClick();

            _turningVoteData.EndReccordPadsToGroup();
        }
    }
}