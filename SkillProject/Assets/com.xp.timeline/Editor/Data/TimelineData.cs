using XPToolchains.Core;
using UnityEngine;
using System.Collections.Generic;

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

        public List<TrackData> Tracks;
    }

    public class TrackData
    {
        public List<ClipData> Clips;
    }

    public class ClipData
    {
        public float StartTime;
        public float EndTime;
        public float DurationTime;
    }
}