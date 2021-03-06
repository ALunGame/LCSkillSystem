using Timeline.Player;
using Timeline.Track;
using Timeline.View;
using UnityEngine;
using XPToolchains.Help;

namespace Timeline.Clip
{
    public class TL_AnimClipPlayer : BaseClipPlayer
    {
        public TL_AnimClipPlayer(BaseClipView clipView) : base(clipView)
        {
        }

        public override void OnStart()
        {
        }

        public override void OnPlaying()
        {
            PlayAnim(clipRunningTime);
        }

        public override void OnEnd()
        {
        }

        private GameObject GetAnimGo()
        {
            TL_AnimTrackView trackView = ((TL_AnimClipView)View).Track as TL_AnimTrackView;
            return trackView.GetTrackGo();
        }

        private void PlayAnim(double animTime)
        {
            GameObject go = GetAnimGo();
            if (go == null)
                return;

            TL_AnimClipData data = (TL_AnimClipData)View.Data;
            string animName = data.animName;
            if (string.IsNullOrEmpty(animName))
                return;

            EditorPlayHelper.PlayAnim(go, animName, (float)animTime);
        }
    }

    /// <summary>
    /// 通用动画片段
    /// </summary>
    [TimelineClip("通用动画片段")]
    [TimelineView(typeof(TL_AnimClipData), typeof(TL_AnimClipPlayer))]
    public class TL_AnimClipView : BaseClipView
    {
        public override string DisplayName => ((TL_AnimClipData)Data).animName;
    }
}