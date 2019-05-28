using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Group
{
    public class GroupName : TextView
    {
        private GroupsInfo groupsInfo;

        public int Id;
        public bool ColorText;

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

            _tx.text = group.Name;

            if(ColorText)
                _tx.color = group.Color;
        }
    }
}