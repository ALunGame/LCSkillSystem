using Timeline.View;
using UnityEngine;
using Timeline.Clip;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Timeline.Track
{
    [TimelineView(typeof(TL_AnimTrackData))]
    [TimelineTrack("通用动画轨道", typeof(TL_AnimClipView))]
    public class TL_AnimTrackView : BaseTrackView
    {
        public override string DisplayName => "通用动画轨道";

        public GameObject GetTrackGo()
        {
            var data = (TL_AnimTrackData)Data;
            if (data.Go.Obj == null)
                return null;
            return data.Go.Obj as GameObject;
        }
    }
}