using Timeline.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;
using XPToolchains.Extension;

namespace Timeline.View
{
    /// <summary>
    /// 轨道视图（一个轨道包含多个片段）
    /// </summary>
    public class BaseTrackView : BaseTimelineView
    {
        #region Static

        public static float TrackOffset = 3;
        private static float TrackHeight = 30;
        private static float TrackHeadWidth = 200;

        #endregion Static

        public Rect TrackViewRect => new Rect(Sequence.SequenceViewRect.x, Sequence.SequenceViewRect.y + ((TrackHeight + TrackOffset) * (TrackIndex)), window.position.width, TrackHeight);

        public Rect TrackHeadViewRect => new Rect(Sequence.SequenceViewRect.x, Sequence.SequenceViewRect.y + ((TrackHeight + TrackOffset) * (TrackIndex)), TrackHeadWidth, TrackHeight);

        #region Virtual

        public virtual Color UnSelectColor { get { return new Color32(23, 99, 166, 100); } }
        public virtual Color SelectColor { get { return new Color32(34, 204, 242, 255); } }
        public virtual Color DisplayNameColor { get { return new Color32(12, 24, 41, 255); } }
        public virtual string DisplayName { get { return "Track"; } }

        public virtual void OnDrawTrack()
        {
            GUI.color = DisplayNameColor;
            EditorGUILayout.LabelField(DisplayName, EditorStylesExtension.MiddleLabelStyle, GUILayout.Height(TrackHeadViewRect.height));
            GUI.color = Color.white;
        }

        public virtual void OnSelect()
        {
            EditorInspectorExtension.DrawObjectInInspector(Data);
        }

        #endregion Virtual

        public int TrackIndex;
        public BaseSequenceView Sequence;
        public List<BaseClipView> Cliplist = new List<BaseClipView>();
        public TrackData data;

        public virtual TrackData Data
        {
            get
            {
                if (data == null)
                    data = new TrackData();
                return data;
            }
        }

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
            var backgroundColor = IsSelected ? SelectColor : UnSelectColor;
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

        public override void OnRunningTimeChange(double runningTime)
        {
            for (int i = 0; i < Cliplist.Count; i++)
            {
                Cliplist[i].OnRunningTimeChange(runningTime);
            }
        }

        public void AddClipView(BaseClipView clipView)
        {
            clipView.Init(window);
            clipView.Track = this;
            Cliplist.Add(clipView);
        }
    }
}