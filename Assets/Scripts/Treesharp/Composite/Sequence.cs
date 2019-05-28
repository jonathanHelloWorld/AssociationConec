using System.Collections.Generic;

namespace InterativaSystem.Treesharp.Composites
{
    public class Sequence : Composite
    {
        public Sequence(List<Node> children) : base(children) { }


        public override NodeStates Tick(Tick tick)
        {
            base.Tick(tick);

            for (var i=0; i<this.children.Count; i++) 
            {
                var status = this.children[i].Execute(tick);
 
                if (status != NodeStates.Success) 
                {
                    return status;
                }
            }
 
            return NodeStates.Success;
        }
    }
}