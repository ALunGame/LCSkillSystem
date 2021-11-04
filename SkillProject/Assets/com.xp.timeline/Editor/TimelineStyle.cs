using UnityEditor;
using UnityEngine;

namespace Timeline
{
    public class TimelineStyle
    {
        private static readonly string EditImgPath = @"Assets/com.xp.timeline/Editor/Images/";

        public static Texture2D LoadEdStyleImg(string imgName)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(EditImgPath + imgName);
        }
    }
}
