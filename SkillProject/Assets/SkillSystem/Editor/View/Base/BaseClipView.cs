using SkillSystem.EDData;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

namespace SkillSystem
{
    public enum ClipDragMode { None, Drag, Left, Right }

    public class BaseClipView : BaseSkillView
    {
        public BaseTrackView Track;

        public Rect ShowRect;
        public Rect LeftRect;
        public Rect RightRect;
        public ClipData ClipData = new ClipData(); 

        public bool IsSelected = false;

        private ClipDragMode dragMode;

        public override void OnInit()
        {
            //Test
            ClipData.StartTime = 1;
            ClipData.EndTime = 10;
            ClipData.DurationTime = ClipData.EndTime - ClipData.StartTime;
        }


        public override void OnDraw()
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            Rect timeAreaRect = timeAreaView.TimeContent;

            //显示区域
            ShowRect    = Track.TrackViewRect;
            ShowRect.x  = timeAreaView.TimeToPixel(ClipData.StartTime);
            float y     = timeAreaView.TimeToPixel(ClipData.EndTime);
            ShowRect.x  = Mathf.Max(ShowRect.x, timeAreaRect.x);
            y = Mathf.Min(y, timeAreaRect.xMax);
            ShowRect.width = y - ShowRect.x;
            ShowRect.height = ShowRect.height - 2;
            if (ShowRect.width < 0) ShowRect.width = 0;

            //显示
            EditorGUI.DrawRect(ShowRect, IsSelected ? Color.red : Color.white);

            //左右侧拖拽
            Rect left   = ShowRect;
            left.x      = ShowRect.x - Mathf.Min(10, ShowRect.width / 4);
            left.x      = Mathf.Max(left.x, timeAreaRect.x);
            left.width  = Mathf.Min(20, ShowRect.width / 2);
            EditorGUIUtility.AddCursorRect(left, MouseCursor.SplitResizeLeftRight);
            LeftRect    = left;
            //EditorGUI.DrawRect(LeftRect, Color.green);

            Rect right  = left;
            right.x     = ShowRect.x + ShowRect.width - Mathf.Min(10, ShowRect.width / 4);
            right.x     = Mathf.Max(right.x, timeAreaRect.x);
            EditorGUIUtility.AddCursorRect(right, MouseCursor.SplitResizeLeftRight);
            RightRect   = right;
            //EditorGUI.DrawRect(RightRect, Color.blue);

            GUILayout.BeginArea(ShowRect);
            OnDrawClip();
            GUILayout.EndArea();

            EDLine.DrawOutline(ShowRect, 1, Color.black);
        }

        public virtual void OnDrawClip()
        {
            EditorGUILayout.LabelField(DisplayName(), GUILayout.Height(20));
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

            ShowRect.x = timeAreaView.TimeToPixel(ClipData.StartTime);
            ShowRect.x += e.delta.x;

            var start2 = timeAreaView.PiexlToTime(ShowRect.x);
            if (start2 >= 0 && start2 <= ClipData.EndTime)
            {
                ClipData.DurationTime -= (start2 - ClipData.StartTime);
                ClipData.StartTime = Mathf.Max(0, start2);
                e.Use();
            }

            OnDragStart();
        }

        private void DragEnd(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();

            ShowRect.x = timeAreaView.TimeToPixel(ClipData.EndTime);
            ShowRect.x += e.delta.x;

            var end = timeAreaView.PiexlToTime(ShowRect.x);
            if (end > ClipData.StartTime)
            {
                ClipData.DurationTime += (end - ClipData.EndTime);
                e.Use();
            }

            OnDragEnd();
        }

        private void Draging(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            ShowRect.x += e.delta.x;
            ClipData.StartTime = timeAreaView.PiexlToTime(ShowRect.x);
            ClipData.StartTime = Mathf.Max(0, ClipData.StartTime);
            e.Use();

            OnDraging();
        } 

        #endregion

        public virtual string DisplayName()
        {
            return "Clip";
        }

        public virtual void OnSelect()
        {

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
            ClipData.EndTime = ClipData.StartTime + ClipData.DurationTime;
        }
    }
}
