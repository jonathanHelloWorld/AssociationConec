using InterativaSystem.Controllers.Run;
using UnityEditor;

namespace InterativaSystem.EditorScripts.Run
{
    [CustomEditor(typeof(RunController))]
    public class RunControllerInspector : Editor
    {
        void Awake()
        {
            var run = target as RunController;

            if(run.StartSet) return;

            run.StartSet = true;
            run.PoolingDistance = 120;
            run.Speed = 20;
            run.RecoveryTime = 0.8f;
        }
    }
}