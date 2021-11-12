using System;
using Timeline.Player;
using UnityEditor;
using UnityEngine;
using XPToolchains.Extension;
using XPToolchains.Help;

namespace Timeline.View
{
    /// <summary>
    /// 片段属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class TimelineClipAttribute : Attribute
    {
        public string menuName;

        /// <summary>
        /// 轨道属性
        /// </summary>
        /// <param name="menuName">创建菜单名</param>
        public TimelineClipAttribute(string menuName)
        {
            this.menuName = menuName;
        }
    }

    public enum ClipDragMode
    { None, Drag, Left, Right }

    /// <summary>
    /// 片段视图（一个轨道包含多个片段）
    /// </summary>
    [TimelineView(typeof(ClipData), typeof(BaseClipPlayer))]
    public class BaseClipView : BaseTimelineView
    {
        #region Display

        public virtual Color UnSelectColor
        { get { return new Color32(109, 140, 171, 255); } }

        public virtual Color SelectColor
        { get { return new Color32(158, 203, 247, 255); } }

        public virtual Color DisplayNameColor
        { get { return new Color32(12, 24, 41, 255); } }

        public virtual string DisplayName
        { get { return "Clip"; } }

        #endregion Display

        private ClipDragMode dragMode;

        public ClipDragMode DragMode
        {
            get { return dragMode; }
            set { dragMode = value; }
        }

        public BaseTrackView Track;

        public Rect ShowRect;
        public Rect LeftRect;
        public Rect RightRect;

        public bool IsSelected = false;

        protected virtual ClipData clipData
        { get { return Data as ClipData; } }

        protected virtual BaseClipPlayer clipPlayer
        { get { return Player as BaseClipPlayer; } }

        public override void OnInit()
        {
        }

        public override void OnDraw()
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            Rect timeAreaRect = timeAreaView.TimeContent;

            //显示区域
            ShowRect = Track.TrackViewRect;
            ShowRect.x = timeAreaView.TimeToPixel(clipData.StartTime);
            float y = timeAreaView.TimeToPixel(clipData.EndTime);
            ShowRect.x = Mathf.Max(ShowRect.x, timeAreaRect.x);
            y = Mathf.Min(y, timeAreaRect.xMax);
            ShowRect.width = y - ShowRect.x;
            ShowRect.height = ShowRect.height - 2;
            if (ShowRect.width < 0) ShowRect.width = 0;

            //显示
            EditorGUI.DrawRect(ShowRect, IsSelected ? SelectColor : UnSelectColor);

            //左右侧拖拽
            Rect left = ShowRect;
            left.x = ShowRect.x - Mathf.Min(10, ShowRect.width / 4);
            left.x = Mathf.Max(left.x, timeAreaRect.x);
            left.width = Mathf.Min(20, ShowRect.width / 2);
            EditorGUIUtility.AddCursorRect(left, MouseCursor.SplitResizeLeftRight);
            LeftRect = left;
            //EditorGUI.DrawRect(LeftRect, Color.green);

            Rect right = left;
            right.x = ShowRect.x + ShowRect.width - Mathf.Min(10, ShowRect.width / 4);
            right.x = Mathf.Max(right.x, timeAreaRect.x);
            EditorGUIUtility.AddCursorRect(right, MouseCursor.SplitResizeLeftRight);
            RightRect = right;
            //EditorGUI.DrawRect(RightRect, Color.blue);

            GUILayout.BeginArea(ShowRect);
            OnDrawClip();
            GUILayout.EndArea();

            EDLine.DrawOutline(ShowRect, 1, Color.black);
        }

        public virtual void OnDrawClip()
        {
            GUI.color = DisplayNameColor;
            EditorGUILayout.LabelField(DisplayName, EditorStylesExtension.MiddleLabelStyle, GUILayout.Height(ShowRect.height));
            GUI.color = Color.white;
        }

        public override void OnHandleEvent(Event evt)
        {
            Vector2 mousePos = evt.mousePosition;
            switch (evt.type)
            {
                case EventType.MouseDown:
                    IsSelected = false;
                    if (LeftRect.Contains(mousePos))
                    {
                        dragMode = ClipDragMode.Left;
                    }
                    else if (RightRect.Contains(mousePos))
                    {
                        dragMode = ClipDragMode.Right;
                    }
                    else if (ShowRect.Contains(mousePos))
                    {
                        dragMode = ClipDragMode.Drag;
                    }
                    else
                    {
                        dragMode = ClipDragMode.None;
                    }
                    if (dragMode != ClipDragMode.None)
                    {
                        IsSelected = true;
                        OnSelect();
                        if (Event.current.button == 1)
                        {
                            GenRightClickMenu();
                        }
                    }
                    break;

                case EventType.MouseUp:
                    dragMode = ClipDragMode.None;
                    break;

                case EventType.MouseDrag:
                case EventType.ScrollWheel:
                    HandleDrag(evt);
                    break;

                default:
                    break;
            }
        }

        public override void OnRunningTimeChange(double runningTime)
        {
            clipPlayer.OnRunningTimeChange(runningTime);
        }

        #region 拖拽处理

        private void HandleDrag(Event e)
        {
            if (dragMode == ClipDragMode.Left)
            {
                DragStart(e);
            }
            else if (dragMode == ClipDragMode.Right)
            {
                DragEnd(e);
            }
            else if (dragMode == ClipDragMode.Drag)
            {
                Draging(e);
            }
            SyncClipData();
        }

        private void DragStart(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();

            ShowRect.x = timeAreaView.TimeToPixel(clipData.StartTime);
            ShowRect.x += e.delta.x;

            var start2 = timeAreaView.PiexlToTime(ShowRect.x);
            if (start2 >= 0 && start2 <= clipData.EndTime)
            {
                clipData.DurationTime -= (start2 - clipData.StartTime);
                clipData.StartTime = Mathf.Max(0, start2);
                e.Use();
            }

            OnDragStart();
        }

        private void DragEnd(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();

            ShowRect.x = timeAreaView.TimeToPixel(clipData.EndTime);
            ShowRect.x += e.delta.x;

            var end = timeAreaView.PiexlToTime(ShowRect.x);
            if (end > clipData.StartTime)
            {
                clipData.DurationTime += (end - clipData.EndTime);
                e.Use();
            }

            OnDragEnd();
        }

        private void Draging(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            ShowRect.x += e.delta.x;
            clipData.StartTime = timeAreaView.PiexlToTime(ShowRect.x);
            clipData.StartTime = Mathf.Max(0, clipData.StartTime);
            e.Use();

            OnDraging();
        }

        #endregion 拖拽处理

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
                Track.RemoveClipView(this);
            });

            //复制
            var copyPaste = EditorGUIUtility.TrTextContent("复制");
            pm.AddItem(copyPaste, false, () =>
            {
                CopyView();
            });

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void CopyView()
        {
            BaseTimelineView clipView = CreateView(GetType());
            clipView.SetData(Data);
            ((BaseClipView)clipView).DragMode = ClipDragMode.None;
            ((BaseClipView)clipView).IsSelected = false;

            ClipData clipData = (ClipData)clipView.Data;
            clipData.StartTime += 2;
            clipData.EndTime += 2;
            clipData.DurationTime = clipData.EndTime - clipData.StartTime;
            Track.AddClipView((BaseClipView)clipView, true);
        }

        public virtual void OnSelect()
        {
            EditorInspectorExtension.DrawObjectInInspector(Data);
        }

        public virtual void OnDragStart()
        {
        }

        public virtual void OnDraging()
        {
        }

        public virtual void OnDragEnd()
        {
        }

        private void SyncClipData()
        {
            clipData.EndTime = clipData.StartTime + clipData.DurationTime;
        }
    }
}