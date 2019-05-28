using InterativaSystem.Controllers.Run;
using UnityEditor;

namespace InterativaSystem.EditorScripts.Run
{
    [CustomEditor(typeof(RunScenarioPullingController))]
    public class ScenarioPoolingInspector : Editor
    {
        void Awake()
        {
            var tgt = target as RunScenarioPullingController;

            if (tgt.StartSet) return;

            tgt.StartSet = true;
            tgt.MinimunInTrack = 6;
            tgt.CanSwitchTracks = false;
            tgt.Amount = 0.5f;
            tgt.InitialOffset = 0;
        }
    }
}