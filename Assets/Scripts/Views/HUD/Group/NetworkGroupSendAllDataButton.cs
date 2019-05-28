using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Group
{
    public class NetworkGroupSendAllDataButton : ButtonView
    {
        private GroupsInfo groups;
        protected override void OnStart()
        {
            base.OnStart();

            groups = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
        }

        protected override void OnClick()
        {
            base.OnClick();
#if HAS_SERVER
            groups.NetworkSendGroups();
#endif
        }
    }
}