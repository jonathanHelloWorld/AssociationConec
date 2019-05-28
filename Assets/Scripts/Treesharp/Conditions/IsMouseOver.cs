using UnityEngine;

namespace InterativaSystem.Treesharp.Conditions
{
    public class IsMouseOver : Condition
    {
        public override NodeStates Tick(Tick tick)
        {
            if (Input.GetMouseButton(0))
            {
                return NodeStates.Success;
            }
            else
            {
                return NodeStates.Failure;
            }
        }
    }
}