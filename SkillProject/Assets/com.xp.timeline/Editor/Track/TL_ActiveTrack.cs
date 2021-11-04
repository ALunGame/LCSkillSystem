using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeline.Data;
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
    public class TL_ActiveTrackView : BaseTrackView
    {
        public override TrackData Data => new TL_ActiveTrackData();
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