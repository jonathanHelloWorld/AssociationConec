using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD
{
    public class ScoreTextTime : TextView
    {
        private ScoreController _scoreController;
        public ConvertTimeType convertType;
        public bool useType = true;
        private ScoreData _scoreData;

#if HAS_SERVER
        public bool useFixedid;
        public string fixedId;
#endif

        protected override void OnStart()
        {
            _scoreController = _controller as ScoreController;
            _scoreController.OnUpdateScore += UpdateData;
        }

        private void UpdateData(ScoreValue value)
        {
            _tx.text = Utils.ConvertRealTime(value.time, convertType);
        }
    }
}