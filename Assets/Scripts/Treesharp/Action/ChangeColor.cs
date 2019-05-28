using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Treesharp.Actions
{
    public class ChangeColor : Action
    {
        public Color color;
        public ChangeColor(Color color)
        {
            this.color = color;
        }

        public override NodeStates Tick(Tick tick)
        {
            base.Tick(tick);

            tick.target.GetComponent<Image>().color = color;

            return NodeStates.Success;
        }
    }
}