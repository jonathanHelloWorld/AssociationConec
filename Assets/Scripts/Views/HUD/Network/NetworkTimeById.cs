using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkTimeById : NetworkTextByIdBase
    {
        protected override void OnStart()
        {
            base.OnStart();

            _score.OnUpdateScore += UpdateData;
        }

        private void UpdateData(ScoreValue value)
        {
            base.UpdateData();

            if (string.IsNullOrEmpty(myUid)) return;

            ScoreValue score = null;

            if (_score.TryGetScore(myUid, out score))
            {
                _tx.text = score.time.ToString("N");
            }
            else
                _tx.text = "0000";
        }

        protected override void UpdateData()
        {
            base.UpdateData();

            if (string.IsNullOrEmpty(myUid)) return;

            ScoreValue score = null;

            if (_score.TryGetScore(myUid, out score))
            {
                _tx.text = score.time.ToString("N");
            }
            else
                _tx.text = "0000";
        }

        protected override void UpdateData(NetworkClientObject client)
        {
            base.UpdateData(client);

            if (string.IsNullOrEmpty(myUid)) return;

            ScoreValue score = null;

            if (_score.TryGetScore(myUid, out score))
            {
                _tx.text = score.time.ToString("N");
            }
            else
                _tx.text = "0000";
        }
    }
}