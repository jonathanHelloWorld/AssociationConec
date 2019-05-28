using InterativaSystem.Models;

namespace InterativaSystem.Views.HUD.Group
{
    public class GroupSetPointsInput : InputView
    {
        public int Id;
        private GroupsInfo groups;

        protected override void OnStart()
        {
            base.OnStart();

            groups = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
        }

        protected override void ValueChanged(string value)
        {
            base.ValueChanged(value);
            UpdateValue(value);
        }

        protected override void EndEdit(string value)
        {
            base.EndEdit(value);
            UpdateValue(value);
        }

        private int points;
        void UpdateValue(string value)
        {
            if (int.TryParse(value, out points) && groups.Groups.Exists(x => x.Id == Id))
                groups.Groups.Find(x => x.Id == Id).Points = points;
            else
            {
                input.text = "";
            }
        }
    }
}