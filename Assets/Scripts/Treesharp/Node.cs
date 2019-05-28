using System.Collections.Generic;

namespace InterativaSystem.Treesharp
{
    public abstract class Node
    {
        public string id;
        public List<Node> children;

        protected virtual void Initialize(List<Node> children) { }

        public virtual NodeStates Execute(Tick tick)
        {
            return NodeStates.Error;
        }

        public virtual void Enter(Tick tick) { }
        public virtual void Open(Tick tick) { }
        public virtual NodeStates Tick(Tick tick)
        {
            return NodeStates.Error;
        }
        public virtual void Close(Tick tick) { }
        public virtual void Exit(Tick tick) { }
    }
}