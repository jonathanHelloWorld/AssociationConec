using InterativaSystem.Models;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Turning
{
    public class TurningButtonStartPadGroupRecoording : ButtonView
    {
        private TurningVoteData _turningVoteData;
        public int GroupId;

        public InputField input;

        protected override void OnStart()
        {
            base.OnStart();

            _turningVoteData = _bootstrap.GetModel(ModelTypes.TurningVote) as TurningVoteData;
        }

        protected override void OnClick()
        {
            base.OnClick();

            if (input == null)
            {
                _turningVoteData.ReccordPadsToGroup(GroupId);
                return;
            }

            if (int.TryParse(input.text, out GroupId))
            {
                _turningVoteData.ReccordPadsToGroup(GroupId);
            }
        }
    }
}