using System.Collections.Generic;

namespace InterativaSystem.Treesharp.Composites
{
    public class Priority : Composite
    {
        public Priority(List<Node> children) : base(children) { }
        public override NodeStates Tick(Tick tick)
        {
            for (var i = 0; i < this.children.Count; i++) 
            {
                var status = this.children[i].Execute(tick);
 
                if (status != NodeStates.Failure) 
                {
                    return status;
                }
            }
 
            return NodeStates.Failure;
        }
    }
}