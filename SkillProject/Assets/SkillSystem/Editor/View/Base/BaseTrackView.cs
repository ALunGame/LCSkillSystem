using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

namespace SkillSystem
{
    /// <summary>
    /// 轨道视图（一个轨道包含多个片段）
    /// </summary>
    public class BaseTrackView : BaseSkillView
    {
        public static float TrackOffset = 3;

        private static Color ColorDuration      = new Color(0.66f, 0.66f, 0.66f, 0.4f);
        private static Color BackgroundColor    = new Color(0.2f, 0.2f, 0.2f, 0.6f);

        private static float TrackHeight        = 30;
        private static float TrackHeadWidth     = 200;

        public BaseSequenceView Sequence;
        public int TrackIndex;

        public Rect TrackViewRect => new Rect(Sequence.SequenceViewRect.x, Sequence.SequenceViewRect.y + ((TrackHeight + TrackOffset) * (TrackIndex)), window.position.width, TrackHeight);

        public Rect TrackHeadViewRect => new Rect(Sequence.SequenceViewRect.x, Sequence.SequenceViewRect.y + ((TrackHeight + TrackOffset) * (TrackIndex)), TrackHeadWidth, TrackHeight);

        public List<BaseClipView> Cliplist = new List<BaseClipView>();
        private bool IsSelected = false;

        public override void OnInit()
        {
            //Test
            BaseClipView clipView = new BaseClipView();
            AddClipView(clipView);
        }

        public override void OnDraw()
        {
            //绘制轨道选择状态
            var backgroundColor = IsSelected ? ColorDuration : BackgroundColor;
            EditorGUI.DrawRect(TrackViewRect, backgroundColor);

            //绘制轨道按钮
            GUILayout.BeginArea(TrackHeadViewRect);
            OnDrawTrack();
            GUILayout.EndArea();

            //绘制轨道按钮边框
            EDLine.DrawOutline(TrackHeadViewRect, 2, Color.gray);

            //绘制片段
            for (int i = 0; i < Cliplist.Count; i++)
            {
                Cliplist[i].OnDraw();
            }
        }

        public override void OnHandleEvent(Event evt)
        {
            Vector2 mousePos = evt.mousePosition;
            switch (evt.type)
            {
                case EventType.MouseDown:
                    IsSelected = false;
                    if (TrackViewRect.Contains(mousePos))
                    {
                        IsSelected = true;
                        OnSelect();
                    }
                    break;
                default:
                    break;
            }

            for (int i = 0; i < Cliplist.Count; i++)
            {
                Cliplist[i].OnHandleEvent(evt);
            }
        }

        //轨道名
        public virtual string DisplayName()
        {
            return "Track";
        }

        public virtual void OnDrawTrack()
        {
            EditorGUILayout.LabelField(DisplayName(), GUILayout.Height(20));
        }

        public virtual void OnSelect()
        {

        }

        public void AddClipView(BaseClipView clipView)
        {
            clipView.Init(window);
            clipView.Track = this;
            Cliplist.Add(clipView);
        }
    }
}
