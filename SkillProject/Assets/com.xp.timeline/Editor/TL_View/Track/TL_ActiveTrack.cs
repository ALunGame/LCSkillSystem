using Timeline.Clip;
using Timeline.View;
using UnityEditor;
using UnityEngine;

namespace Timeline.Track
{
    /// <summary>
    /// 节点显示隐藏轨道
    /// </summary>
    [TimelineView(typeof(TL_ActiveTrackData))]
    [TimelineTrack("显隐轨道", typeof(TL_ActiveClipView))]
    public class TL_ActiveTrackView : BaseTrackView
    {
        public override string DisplayName => "显隐轨道";

        public GameObject GetTrackGo()
        {
            var data = (TL_ActiveTrackData)Data;
            if (data.Go.Obj == null)
                return null;
            return data.Go.Obj as GameObject;
        }
    }
}