using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Turning
{
    public class NetworkTurningSendVotesToClients : ButtonView
    {
#if HAS_SERVER
        private TurningVoteData turningVote;
#endif

        protected override void OnStart()
        {
            base.OnStart();

#if HAS_SERVER
            turningVote = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
#endif
        }

        protected override void OnClick()
        {
            base.OnClick();

#if HAS_SERVER
            turningVote.NetworkSendVotes();
#endif
        }
    }
}