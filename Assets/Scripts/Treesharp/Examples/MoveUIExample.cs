using System.Collections.Generic;
using InterativaSystem.Treesharp.Actions;
using InterativaSystem.Treesharp.Composites;
using InterativaSystem.Treesharp.Conditions;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Treesharp.Examples
{
    public class MoveUIExample : MonoBehaviour
    {
        public Vector3 Limits;

        private Behaviour _tree;
        private Blackboard _blackboard;

        void Awake()
        {
            _blackboard = new Blackboard();
            _tree = new Behaviour();

            _tree.root = new Priority(
                new List<Node>
                {
                    new Sequence(new List<Node>()
                    {
                        new IsMouseOver(),
                        new MemorySequence(new List<Node>()
                        {
                            new ChangeColor(Color.red),
                            new Wait(0.5f),
                            new ChangePosition(),
                            new ChangeColor(Color.blue)
                        })
                    }),
                    new ChangeColor(Color.blue),
                });

            Debug.Log(JsonConvert.SerializeObject(_tree, Formatting.Indented));
        }

        void Update()
        {
            _tree.Tick(this.gameObject, _blackboard);
        }
    }
}