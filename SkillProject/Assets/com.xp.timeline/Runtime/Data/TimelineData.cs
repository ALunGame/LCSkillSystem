using System.Collections.Generic;
using UnityEngine;
using XPToolchains;

namespace Timeline
{
    public class SequenceData
    {
        /// <summary>
        /// Timeline资源名
        /// </summary>
        public string Name;

        /// <summary>
        /// Timeline持续时间
        /// </summary>
        public float DurationTime;

        /// <summary>
        /// Timeline所属的节点
        /// </summary>
        [UnityAssetType(typeof(GameObject), true)]
        public UnityObjectAsset Go;

        [HideInInspector]
        public List<TrackData> Tracks = new List<TrackData>();
    }

    public class TrackData
    {
        [HideInInspector]
        public List<ClipData> Clips = new List<ClipData>();
    }

    public class ClipData
    {
        public float StartTime;
        public float EndTime;
        public float DurationTime;
    }
}