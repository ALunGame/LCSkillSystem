using Timeline.Player;
using Timeline.Track;
using Timeline.View;
using UnityEngine;

namespace Timeline.Clip
{
    public class TL_ActivePlayer : BaseClipPlayer
    {
        public TL_ActivePlayer(BaseClipView clipView) : base(clipView)
        {
        }

        public override void OnStart()
        {
            GameObject go = GetAnimGo();
            if (go == null)
                return;

            TL_ActiveClipData data = (TL_ActiveClipData)View.Data;
            go.SetActive(data.isActive);
        }

        public override void OnEnd()
        {
            GameObject go = GetAnimGo();
            if (go == null)
                return;
            TL_ActiveClipData data = (TL_ActiveClipData)View.Data;
            go.SetActive(!data.isActive);
        }

        private GameObject GetAnimGo()
        {
            TL_ActiveTrackView trackView = ((TL_ActiveClipView)View).Track as TL_ActiveTrackView;
            return trackView.GetTrackGo();
        }
    }

    /// <summary>
    /// 节点显示隐藏片段
    /// </summary>
    [TimelineClip("显示隐藏片段")]
    [TimelineView(typeof(TL_ActiveClipData), typeof(TL_ActivePlayer))]
    public class TL_ActiveClipView : BaseClipView
    {
        public override string DisplayName => ((TL_ActiveClipData)Data).isActive.ToString();
    }
}