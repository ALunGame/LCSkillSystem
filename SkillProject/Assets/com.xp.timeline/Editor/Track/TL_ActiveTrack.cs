using Timeline.Clip;
using Timeline.View;
using UnityEditor;
using UnityEngine;

namespace Timeline.Track
{
    public class TL_ActiveTrackData : TrackData
    {
        public string goAssetPath;
    }

    /// <summary>
    /// 节点显示隐藏轨道
    /// </summary>
    [TimelineView(typeof(TL_ActiveTrackData))]
    [TimelineTrack("显隐轨道", typeof(TL_ActiveClipView))]
    public class TL_ActiveTrackView : BaseTrackView
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
            var data = (TL_ActiveTrackData)Data;
            if (string.IsNullOrEmpty(data.goAssetPath))
                return null;
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(data.goAssetPath);
            return go;
        }
    }
}