using InterativaSystem.Controllers;

namespace InterativaSystem.Views.HUD.Group
{
    public class GroupRankingPanel : CanvasGroupView
    {
        private ConquerController conquerController;

        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
            conquerController.ShowRank += ShowRank;
            conquerController.HideRank += HideRank;

            Hide();
        }

        private void ShowRank()
        {
            Show();
        }
        private void HideRank()
        {
            Hide();
        }
    }
}