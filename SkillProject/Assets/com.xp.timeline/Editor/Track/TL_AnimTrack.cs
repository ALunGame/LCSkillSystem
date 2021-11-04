using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeline.View;
using Timeline.Data;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Timeline.Track
{
    public class TL_AnimTrackData : TrackData
    {
        public string goAssetPath;
    }

    public class TL_AnimTrackView : BaseTrackView
    {
        public override TrackData Data => new TL_AnimTrackData();
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