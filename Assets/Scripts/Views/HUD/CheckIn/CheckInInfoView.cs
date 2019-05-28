using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.CheckIn
{
    public class CheckInInfoView : CanvasGroupView
    {
        public string Name;
        public int Index;

        public Text TxName;
        public Text TxIndex;

        public void UpdataInfo()
        {
            TxName.text = Name;
            TxIndex.text = Index.ToString("000");
        }
    }
}