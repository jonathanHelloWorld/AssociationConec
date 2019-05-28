using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Treesharp
{
    public class Tick
    {
        public Tick()
        {
            Initialize();
        }

        public List<Node> openNodes;
        public int nodeCount;
        public Blackboard blackboard;
        public GameObject target;
        public Behaviour tree;

        protected virtual void Initialize()
        {
            tree       = null;
            openNodes  = new List<Node>();
            nodeCount  = 0;
            //debug      = null;
            target     = null;
            blackboard = null;
        }
        
        public void EnterNode(Node node)
        {
            this.nodeCount++;
            this.openNodes.Add(node);
            // call debug here
        }
        
        public void OpenNode(Node node)
        {
            //Debug.Log("Openning " + node);
            // call debug here
        }
 
        public void TickNode(Node node) 
        {
            // call debug here
        }
 
        public void CloseNode(Node node)
        {
            //Debug.Log("Closing " + node);
            // call debug here
            this.openNodes.Remove(node);
        }
 
        public void ExitNode(Node node)
        {
            // call debug here
        }
    }
}