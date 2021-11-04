using Timeline.Data;
using Timeline.Player;
using UnityEditor;
using UnityEngine;
using XPToolchains.Extension;
using XPToolchains.Help;

namespace Timeline.View
{
    public enum ClipDragMode { None, Drag, Left, Right }

    public class BaseClipView : BaseTimelineView
    {
        public BaseTrackView Track;

        public Rect ShowRect;
        public Rect LeftRect;
        public Rect RightRect;

        public bool IsSelected = false;

        private ClipDragMode dragMode;

        public virtual Color UnSelectColor { get { return new Color32(109, 140, 171, 255); } }
        public virtual Color SelectColor { get { return new Color32(158, 203, 247, 255); } }
        public virtual Color DisplayNameColor { get { return new Color32(12, 24, 41, 255); } }
        public virtual string DisplayName { get { return "Clip"; } }

        private ClipData data;

        public virtual ClipData Data
        {
            get
            {
                if (data == null)
                    data = new ClipData();
                return data;
            }
        }

        private BaseClipPlayer clipPlayer;

        public virtual BaseClipPlayer ClipPlayer
        {
            get
            {
                if (clipPlayer == null)
                    clipPlayer = new BaseClipPlayer(this);
                return clipPlayer;
            }
        }

        public override void OnInit()
        {
            //Test
            Data.StartTime = 1;
            Data.EndTime = 10;
            Data.DurationTime = Data.EndTime - Data.StartTime;
        }

        public override void OnDraw()
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            Rect timeAreaRect = timeAreaView.TimeContent;

            //显示区域
            ShowRect = Track.TrackViewRect;
            ShowRect.x = timeAreaView.TimeToPixel(Data.StartTime);
            float y = timeAreaView.TimeToPixel(Data.EndTime);
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
                    break;

                case EventType.MouseUp:
                    if (dragMode == ClipDragMode.Drag)
                    {
                        IsSelected = true;
                        OnSelect();
                        evt.Use();
                    }
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
            ClipPlayer.OnRunningTimeChange(runningTime);
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

            ShowRect.x = timeAreaView.TimeToPixel(Data.StartTime);
            ShowRect.x += e.delta.x;

            var start2 = timeAreaView.PiexlToTime(ShowRect.x);
            if (start2 >= 0 && start2 <= Data.EndTime)
            {
                Data.DurationTime -= (start2 - Data.StartTime);
                Data.StartTime = Mathf.Max(0, start2);
                e.Use();
            }

            OnDragStart();
        }

        private void DragEnd(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();

            ShowRect.x = timeAreaView.TimeToPixel(Data.EndTime);
            ShowRect.x += e.delta.x;

            var end = timeAreaView.PiexlToTime(ShowRect.x);
            if (end > Data.StartTime)
            {
                Data.DurationTime += (end - Data.EndTime);
                e.Use();
            }

            OnDragEnd();
        }

        private void Draging(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            ShowRect.x += e.delta.x;
            Data.StartTime = timeAreaView.PiexlToTime(ShowRect.x);
            Data.StartTime = Mathf.Max(0, Data.StartTime);
            e.Use();

            OnDraging();
        }

        #endregion 拖拽处理

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
            Data.EndTime = Data.StartTime + Data.DurationTime;
        }
    }
}