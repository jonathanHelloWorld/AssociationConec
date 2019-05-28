using InterativaSystem.Controllers.Run;
using UnityEditor;

namespace InterativaSystem.EditorScripts.Run
{
    [CustomEditor(typeof(RunCoinsPullingController))]
    public class CoinPoolingInspector : Editor
    {
        void Awake()
        {
            var tgt = target as RunCoinsPullingController;

            if (tgt.StartSet) return;

            tgt.StartSet = true;
            tgt.MinimunInTrack = 5;
            tgt.CanSwitchTracks = true;
            tgt.Amount = 0.04f;
            tgt.InitialOffset = 20;
        }
    }
}