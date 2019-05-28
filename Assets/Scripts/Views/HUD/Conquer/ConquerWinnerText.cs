using InterativaSystem.Controllers;
using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Conquer
{
    public class ConquerWinnerText : TextView
    {
        private ConquerController conquerController;
        private GroupsInfo groupsInfo;
        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if(groupsInfo.Groups.Exists(x => x.Id == conquerController.winner))
                _tx.text = groupsInfo.Groups.Find(x => x.Id == conquerController.winner).Name;
            else
                _tx.text = "";
        }
    }
}