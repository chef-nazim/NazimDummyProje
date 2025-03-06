using System;
using NCG.template._NCG.Core.Model;
using NCG.template.Scripts.Item;
using UnityEngine;

namespace NCG.template.models
{
    [Serializable]
    public class GoalItemModel : BaseItemModel
    {
        public GoalItem Item;
        public int ColorID;
        public int GoalCount;

        public void SetParent(GameObject levelModelGoalParent)
        {
            Item.SetParent(levelModelGoalParent.transform);
        }

        public void SetMaterial(int vColorID)
        {
            ColorID = vColorID;
            Item.SetItemSprite(ColorID);
        }

        public void SetGoalCount(int vCount)
        {
            GoalCount = vCount;
            Item.SetCountText(GoalCount);
        }
        public void OpenStackIcon(Color color)
        {
            Item.OpenStackIcon(color);
        }
    }
}