
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InterativaSystem.Treesharp
{
    public enum NodeStates
    {
        Success = 0,
        Failure = 1,
        Running = 2,
        Error,
        Null
    }

    public class Behaviour
    {
        private const string HexDigits = "0123456789abcdef";

        public string id;
        public Node root;

        public Behaviour()
        {
            Initialize();
        }

        public static string CreateUID()
        {
            var uid = Guid.NewGuid();
            /*
            var s = new string[36];

            var rnd = new System.Random();

            for (int i = 0; i < 36; i++)
            {
                var str = HexDigits.Substring((int)(Mathf.Floor((float)rnd.NextDouble()*0x10)), 1);
                s[i] = str;
            }

            // bits 12-15 of the time_hi_and_version field to 0010
            s[14] = "4";

            // bits 6-7 of the clock_seq_hi_and_reserved to 01
            int nt = 1;
            try
            {
                nt = Convert.ToInt16(s[19]) & 0x3 | 0x8;
            }
            catch (Exception) { }

            s[19] = HexDigits.Substring(nt, 1);

            s[8] = s[13] = s[18] = s[23] = "-";

            StringBuilder builder = new StringBuilder();
            foreach (string value in s)
            {
                builder.Append(value);
                builder.Append("");
            }
            /**/
            var uuid = uid.ToString();//builder.ToString();
            return uuid;
        }

        protected virtual void Initialize()
        {
            id = CreateUID();
            root = null;
        }

        public void Tick(GameObject target, Blackboard blackboard)
        {
            var tick = new Tick();
            tick.target = target;
            tick.blackboard = blackboard;
            tick.tree = this;

            /* TICK NODE */
            this.root.Execute(tick);

            /* CLOSE NODES FROM LAST TICK, IF NEEDED */
            List<Node> lastOpenNodes;
            lastOpenNodes = new List<Node>();

            try
            {
                lastOpenNodes = (List<Node>) blackboard.Get("openNodes", id).value;
            }
            catch (Exception){ }

            var currOpenNodes = new List<Node>(tick.openNodes);

            // does not close if it is still open in this tick
            var start = 0;
            int n = 0;

            if (lastOpenNodes == null)
                n = currOpenNodes.Count;
            else
                n = Math.Min(lastOpenNodes.Count, currOpenNodes.Count);


            if (lastOpenNodes != null)
            {
                for (int i = 0; i < n; i++)
                {
                    start = i + 1;
                    if ((Node) lastOpenNodes[i] != currOpenNodes[i])
                    {
                        break;
                    }
                }
            }
            else
                start = currOpenNodes.Count;

            // close the nodes
            if (lastOpenNodes != null)
                for (var i = lastOpenNodes.Count - 1; i >= start; i--)
                {
                    lastOpenNodes[i].Close(tick);
                }

            /* POPULATE BLACKBOARD */
            blackboard.Set("openNodes", currOpenNodes, id);
            blackboard.Set("nodeCount", tick.nodeCount, id);
        }
    }
}