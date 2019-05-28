using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Network
{
    public class NetworkScoreById : NetworkTextByIdBase
    {
        protected override void OnStart()
        {
            base.OnStart();

            _score.OnUpdateScore += UpdateData;
        }

        private void UpdateData(ScoreValue value)
        {
            base.UpdateData();

            _tx.text = "";

            if (string.IsNullOrEmpty(myUid)) return;
            if (!_network.GetConnection(myUid).isGamePrepared) return;

            ScoreValue score = null;

            if (_score.TryGetScore(myUid, out score))
            {
                _tx.text = score.value.ToString("000");
            }
            else
                _tx.text = "000";
        }

        protected override void UpdateData()
        {
            base.UpdateData();

            _tx.text = "";

            if (string.IsNullOrEmpty(myUid)) return;
            if (!_network.GetConnection(myUid).isGamePrepared) return;

            ScoreValue score = null;

            if (_score.TryGetScore(myUid, out score))
            {
                _tx.text = score.value.ToString("000");
            }
            else
                _tx.text = "000";
        }

        protected override void UpdateData(NetworkClientObject client)
        {
            base.UpdateData(client);

            _tx.text = "";

            if (string.IsNullOrEmpty(myUid)) return;
            if (!_network.GetConnection(myUid).isGamePrepared) return;

            ScoreValue score = null;

            if (_score.TryGetScore(myUid, out score))
            {
                _tx.text = score.value.ToString("000");
            }
            else
                _tx.text = "000";
        }
    }
}