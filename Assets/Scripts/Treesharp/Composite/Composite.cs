using System.Collections.Generic;
using InterativaSystem.Treesharp.Nodes;

namespace InterativaSystem.Treesharp.Composites
{
    public class Composite : BaseNode
    {
        public Composite(List<Node> children) : base(children) { }
    }
}