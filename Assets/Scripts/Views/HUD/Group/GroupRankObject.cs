using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Group
{
    public class GroupRankObject : GenericView
    {
        private ConquerController conquerController;
        private GroupsInfo groupInfo;

        public int Id;

        protected override void OnStart()
        {
            base.OnStart();

            conquerController = _bootstrap.GetController(_controllerType) as ConquerController;
            conquerController.ShowRank += UpdatePos;

            groupInfo = _bootstrap.GetModel(ModelTypes.Group) as GroupsInfo;
        }

        protected override void OnUpdate()
        {
            UpdatePos();
        }

        private void UpdatePos()
        {
            var grs = groupInfo.Groups.OrderByDescending(x=>x.AreaPoints).ToList();

            transform.SetSiblingIndex(grs.FindIndex(x=>x.Id == Id));
        }
    }
}