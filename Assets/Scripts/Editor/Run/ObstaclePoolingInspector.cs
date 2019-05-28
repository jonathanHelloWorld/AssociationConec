using InterativaSystem.Controllers.Run;
using UnityEditor;

namespace InterativaSystem.EditorScripts.Run
{
    [CustomEditor(typeof(RunObstaclesPullingController))]
    public class ObstaclePoolingInspector : Editor
    {
        void Awake()
        {
            var tgt = target as RunObstaclesPullingController;

            if (tgt.StartSet) return;

            tgt.StartSet = true;
            tgt.MinimunInTrack = 1;
            tgt.CanSwitchTracks = true;
            tgt.Amount = 0.06f;
            tgt.InitialOffset = 100;
        }
    }
}