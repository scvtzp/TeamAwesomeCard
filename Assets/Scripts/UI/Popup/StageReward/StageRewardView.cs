using Manager;
using UI.UIBase;

namespace UI.Popup.StageReward
{
    public class StageRewardView : PopupBase
    {
        public override void HideEnd()
        {
            base.HideEnd();
            StageManager.Instance.SetStage();
        }
    }
}