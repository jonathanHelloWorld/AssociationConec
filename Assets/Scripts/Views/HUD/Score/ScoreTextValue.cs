using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.HUD;

namespace InterativaSystem.Views.HUD
{
    public class ScoreTextValue : DynamicText
    {
        //TODO Yuri: criar models e views para representar itens coletaveis de tipos diferentes

        private ScoreController _scoreController;

#if HAS_SERVER
        public bool useFixedid;
        public string fixedId;
#endif

        protected override void OnStart()
        {
            _scoreController = _controller as ScoreController;
            _scoreController.OnUpdateScore += UpdateData;
        }

        protected override void UpdateData(ScoreValue value)
        {
            string nText = value.value.ToString(format);
            UpdateText(nText);
        }
    }
}