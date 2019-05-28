using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace InterativaSystem.Treesharp.Nodes
{
    public class BaseNode : Node
    {
        public BaseNode()
        {
            //Init here
            Initialize(null);
        }
        public BaseNode(List<Node> children)
        {
            //Init here
            Initialize(children);
        }

        protected override void Initialize(List<Node> children)
        {
            id = Behaviour.CreateUID();
 
            this.children = new List<Node>();

            if (children == null) return;

            for (var i = 0; i < children.Count; i++) 
                this.children.Add(children[i]);
        }

        public override NodeStates Execute(Tick tick)
        {
            /* ENTER */
            Enter(tick);

            var data = tick.blackboard.Get("isOpen", id, this.id).value;

            bool isOpen = false;

            try
            {
                isOpen = (bool) data;
            }
            catch (Exception){ }

            /* OPEN */
            if (!isOpen)
            {
                Open(tick);
            }
 
            /* TICK */
            var status = Tick(tick);
 
            /* CLOSE */
            if (status != NodeStates.Running) 
            {
                Close(tick);
            }
 
            /* EXIT */
            Exit(tick);
 
            return status;
        }
        
        // Wrapper functions
        public override void Enter(Tick tick)
        {
            tick.EnterNode(this);
            //Enter(tick);
        }

        public override void Open(Tick tick)
        {
            tick.OpenNode(this);
            tick.blackboard.Set("isOpen", true, id, this.id);
            //Open(tick);
        }

        public override NodeStates Tick(Tick tick)
        {
            tick.TickNode(this);
            return NodeStates.Null;
        }

        public override void Close(Tick tick)
        {
            tick.CloseNode(this);
            tick.blackboard.Set("isOpen", false, id, this.id);
            //Close(tick);
        }

        public override void Exit(Tick tick)
        {
            tick.ExitNode(this);
            //Exit(tick);
        }
    }
}