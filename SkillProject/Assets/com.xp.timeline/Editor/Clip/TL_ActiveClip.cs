using Timeline.Data;
using Timeline.Player;
using Timeline.Track;
using Timeline.View;
using UnityEngine;

namespace Timeline.Clip
{
    public class TL_ActiveClipData : ClipData
    {
        public bool isActive;
    }

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

            TL_ActiveClipData data = (TL_ActiveClipData)ClipView.Data;
            go.SetActive(data.isActive);
        }

        public override void OnEnd()
        {
            GameObject go = GetAnimGo();
            if (go == null)
                return;
            TL_ActiveClipData data = (TL_ActiveClipData)ClipView.Data;
            go.SetActive(!data.isActive);
        }

        private GameObject GetAnimGo()
        {
            TL_ActiveTrackView trackView = (TL_ActiveTrackView)ClipView.Track;
            return trackView.GetTrackGo();
        }
    }

    /// <summary>
    /// 节点显示隐藏片段
    /// </summary>
    public class TL_ActiveClipView : BaseClipView
    {
        public override ClipData Data => new TL_ActiveClipData();
        public override BaseClipPlayer ClipPlayer => new TL_ActivePlayer(this);

        public override string DisplayName => ((TL_ActiveClipData)Data).isActive.ToString();
    }
}