using UnityEngine;
using System.Collections;

namespace InterativaSystem.Views.Events
{
    public class EventMoveTo : ExecuteEvent
    {
        [Space(10f)]
        public AgentType agent = AgentType.NavMeshAgent;
        public Transform moveToTarget;
        public Transform objectToMove;

        protected override void RunEvent()
        {
            MoveTo();
        }

        void MoveTo()
        {
            EventStart();

            switch(agent)
            {
                case AgentType.NavMeshAgent:
                    StartCoroutine(AgentMoveTo());
                    break;
                case AgentType.CharacterController:
                    StartCoroutine(CharacterMoveTo());
                    break;
            }
        }

        IEnumerator AgentMoveTo()
        {
            UnityEngine.AI.NavMeshAgent navAgent = objectToMove.GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (navAgent != null)
            {
                ToggleController();

                Vector3 targetPos = moveToTarget.position;
                targetPos.y = objectToMove.position.y;

                navAgent.SetDestination(targetPos);

                while (true)
                {
                    EventRepeat();

                    if (Vector3.Distance(objectToMove.transform.position, targetPos) <= 0.4f)
                        break;

                    yield return null;
                }

                navAgent.Stop();
                EventEnd();
            }
        }

        IEnumerator CharacterMoveTo()
        {
            CharacterController charPlayer = objectToMove.GetComponent<CharacterController>();

            if (charPlayer != null)
            {
                ToggleController();

                Vector3 targetPos = moveToTarget.position;
                targetPos.y = objectToMove.position.y;

                while (true)
                {
                    EventRepeat();

                    Vector3 heading = targetPos - objectToMove.transform.position;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;

                    charPlayer.SimpleMove(direction);
                    Debug.DrawRay(objectToMove.position, direction, Color.blue);
                    if (distance <= 0.4f)
                        break;

                    yield return null;
                }

                EventEnd();
            }
        }

        void ToggleController()
        {
            CharacterController charPlayer = objectToMove.GetComponent<CharacterController>();
            UnityEngine.AI.NavMeshAgent navAgent = objectToMove.GetComponent<UnityEngine.AI.NavMeshAgent>();

            switch (agent)
            {
                case AgentType.NavMeshAgent:
                    if (navAgent != null)
                    {
                        navAgent.enabled = true;
                        navAgent.Resume();
                    }

                    break;
                case AgentType.CharacterController:
                    if (navAgent != null && navAgent.isOnNavMesh)
                    {
                        navAgent.Stop();
                        navAgent.enabled = false;
                    }

                    break;
            }
        }
    }

    public enum AgentType { CharacterController, NavMeshAgent }
}