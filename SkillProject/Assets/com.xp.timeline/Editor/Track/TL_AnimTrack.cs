using Timeline.View;
using UnityEngine;
using Timeline.Clip;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Timeline.Track
{
    public class TL_AnimTrackData : TrackData
    {
        public string goAssetPath;
    }

    [TimelineView(typeof(TL_AnimTrackData))]
    [TimelineTrack("动画轨道", typeof(TL_AnimClipView))]
    public class TL_AnimTrackView : BaseTrackView
    {
        public override string DisplayName => GetTrackGoName();

        public string GetTrackGoName()
        {
            GameObject go = GetTrackGo();
            if (go == null)
            {
                return "默认节点";
            }
            return go.name;
        }

        public GameObject GetTrackGo()
        {
            var data = (TL_AnimTrackData)Data;
            if (string.IsNullOrEmpty(data.goAssetPath))
                return null;
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(data.goAssetPath);
            return go;
        }
    }
}