using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.Extension;
using XPToolchains.Help;

namespace Timeline.View
{
    /// <summary>
    /// 轨道属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class TimelineTrackAttribute : Attribute
    {
        public string menuName;

        public Type clipViewType;

        /// <summary>
        /// 轨道属性
        /// </summary>
        /// <param name="menuName">创建菜单名</param>
        /// <param name="clipType">片段类型</param>
        public TimelineTrackAttribute(string menuName, Type clipType)
        {
            this.menuName = menuName;
            this.clipViewType = clipType;
        }
    }

    /// <summary>
    /// 轨道视图（一个轨道包含多个片段）
    /// </summary>
    [TimelineView(typeof(TrackData))]
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

        private bool IsSelected = false;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private void GenRightClickMenu()
        {
            GenericMenu pm = new GenericMenu();

            //删除
            var delPaste = EditorGUIUtility.TrTextContent("删除");
            pm.AddItem(delPaste, false, () =>
            {
                Sequence.RemoveTrackView(this);
            });

            //复制
            var copyPaste = EditorGUIUtility.TrTextContent("复制");
            pm.AddItem(copyPaste, false, () =>
            {
                CopyView();
            });

            TimelineTrackAttribute trackAttribute = null;
            if (AttributeHelper.TryGetTypeAttribute(GetType(), out trackAttribute))
            {
                Type clipType = trackAttribute.clipViewType;
                TimelineClipAttribute clipAttribute = null;
                if (AttributeHelper.TryGetTypeAttribute(clipType, out clipAttribute))
                {
                    string menuName = clipAttribute.menuName;
                    var paste = EditorGUIUtility.TrTextContent(menuName);
                    pm.AddItem(paste, false, () =>
                    {
                        CreateClipView(clipType);
                    });
                }
            }

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void CreateClipView(Type clipType)
        {
            BaseClipView clipView = CreateView(clipType) as BaseClipView;
            ClipData clipData = (ClipData)clipView.Data;
            clipData.StartTime = 0;
            clipData.EndTime = 1;
            clipData.DurationTime = clipData.EndTime - clipData.StartTime;
            AddClipView(clipView);
        }

        private void CopyView()
        {
            BaseTimelineView trackView = CreateView(GetType());
            trackView.SetData(Data);
            Sequence.AddTrackView((BaseTrackView)trackView);
        }

        public override void OnInit()
        {
            TrackData data = (TrackData)Data;
            for (int i = 0; i < data.Clips.Count; i++)
            {
                BaseClipView clipView = CreateView(data.Clips[i]) as BaseClipView;
                AddClipView(clipView, false);
            }
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
                    if (TrackViewRect.Contains(mousePos))
                    {
                        IsSelected = true;
                        OnSelect();
                        if (Event.current.button == 1)
                        {
                            GenRightClickMenu();
                        }
                    }
                    else
                    {
                        IsSelected = false;
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

        public virtual void OnAddClipView(BaseClipView clipView)
        {
        }

        public void AddClipView(BaseClipView clipView, bool savaData = true)
        {
            clipView.Init(window);
            clipView.Track = this;
            Cliplist.Add(clipView);
            OnAddClipView(clipView);
            if (savaData)
            {
                TrackData data = (TrackData)Data;
                data.Clips.Add((ClipData)clipView.Data);
            }
        }

        public void RemoveClipView(BaseClipView clipView)
        {
            for (int i = 0; i < Cliplist.Count; i++)
            {
                if (Cliplist[i].Equals(clipView))
                {
                    Cliplist.RemoveAt(i);
                    break;
                }
            }
            TrackData data = (TrackData)Data;
            data.Clips.Remove((ClipData)clipView.Data);
        }
    }
}