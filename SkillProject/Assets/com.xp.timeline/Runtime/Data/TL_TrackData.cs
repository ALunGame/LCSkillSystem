using UnityEngine;
using XPToolchains;

namespace Timeline
{
    public class TL_ActiveTrackData : TrackData
    {
        [UnityAssetType(typeof(GameObject), true)]
        public UnityObjectAsset Go;
    }

    public class TL_AnimTrackData : TrackData
    {
        [UnityAssetType(typeof(GameObject), true)]
        public UnityObjectAsset Go;
    }
}