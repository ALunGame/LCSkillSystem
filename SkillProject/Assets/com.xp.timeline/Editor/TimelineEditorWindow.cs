using System.Collections.Generic;
using Timeline.View;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    public class TimelineEditorWindow : EditorWindow
    {
        //上层工具窗口
        public readonly Rect ToolbarSize = new Rect(0, 0, 0, 10);

        //添加轨道窗口
        public readonly Rect AddTrackSize = new Rect(0, 0, 200, 10);

        //轨道列表窗口
        public readonly Rect TrackListSize = new Rect(0, 50, 0, 0);

        public readonly Rect LeftTimeAreaSize = new Rect(220, 20, 0, 24);
        public readonly Rect RightTimeAreaSize = new Rect(0, 10, 280, 40);

        private List<BaseTimelineView> PartialViews = new List<BaseTimelineView>() { new TopToolbarView(), new TimeAreaView(), new AddTrackView(), new BaseSequenceView() };

        public Rect WinArea { get; set; }

        public bool IsPlayingSkill = false;
        public bool IsLockDragHeaderArrow = false;      //标尺锁定
        public double CutOffTime;                       //标尺时间

        private double lastUpdateTime;
        private double runningTime;

        /// <summary>
        /// 运行时间
        /// </summary>
        public double RunningTime
        {
            get
            {
                return runningTime;
            }
            set
            {
                runningTime = value;
                for (int i = 0; i < PartialViews.Count; i++)
                {
                    PartialViews[i].SyncRunningTime(runningTime);
                }
            }
        }

        public void Init()
        {
            OnInit();
        }

        public void OnEnable()
        {
            Init();
            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].Init(this);
            }
            EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
            lastUpdateTime = (float)EditorApplication.timeSinceStartup;
        }

        public void OnGUI()
        {
            if (PartialViews == null)
                return;
            WinArea = position;
            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].OnDraw();
            }

            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].OnHandleEvent(Event.current);
            }
        }

        public void OnDestroy()
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                PartialViews[i].OnDestroy();
            }
            EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
        }

        private void OnEditorUpdate()
        {
            if (!Application.isPlaying && IsPlayingSkill)
            {
                float delta = 1.0f / 30;
                double fTime = (float)EditorApplication.timeSinceStartup - lastUpdateTime;

                if (fTime > delta)
                {
                    RunningTime += delta;
                    BaseSequenceView sequenceView = GetPartialView<BaseSequenceView>();

                    if (RunningTime >= sequenceView.Data.DurationTime)
                    {
                        RunningTime = 0;
                        //IsPlayingSkill = false;
                    }
                    lastUpdateTime = (float)EditorApplication.timeSinceStartup;
                }
            }
            Repaint();
        }

        #region Virtual

        public virtual void OnInit()
        {
        }

        #endregion Virtual

        #region 接口

        public void AddPartialView(BaseTimelineView skillView)
        {
            PartialViews.Add(skillView);
        }

        public T GetPartialView<T>() where T : BaseTimelineView
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                if (PartialViews[i] is T)
                {
                    return (T)PartialViews[i];
                }
            }
            return null;
        }

        public void RemovePartialView<T>() where T : BaseTimelineView
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                if (PartialViews[i] is T)
                {
                    PartialViews.RemoveAt(i);
                }
            }
        }

        //设置播放状态
        public void SetSkillEdPlay(bool reqPlay)
        {
            if (IsPlayingSkill == reqPlay)
                return;

            IsPlayingSkill = reqPlay;
        }

        #endregion 接口
    }
}