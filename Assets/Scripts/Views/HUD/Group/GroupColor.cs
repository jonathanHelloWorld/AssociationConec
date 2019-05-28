using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Group
{
    public class GroupColor : ImageView
    {
        private GroupsInfo groupsInfo;

        public int Id;

        protected override void OnStart()
        {
            base.OnStart();

            groupsInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;

            if(groupsInfo ==  null || !groupsInfo.Groups.Exists(x => x.Id == Id))
                gameObject.SetActive(false);

            group = groupsInfo.Groups.Find(x => x.Id == Id);
        }

        private Models.Group group;
        protected override void OnLateUpdate()
        {
            base.OnLateUpdate();

            _image.color = group.Color;
        }
    }
}