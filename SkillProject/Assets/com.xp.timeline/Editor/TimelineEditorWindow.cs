using System.Collections.Generic;
using Timeline.Serialize;
using Timeline.View;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    public abstract class TimelineEditorWindow : EditorWindow
    {
        //�ϲ㹤�ߴ���
        public readonly Rect ToolbarSize = new Rect(0, 0, 0, 10);

        //��ӹ������
        public readonly Rect AddTrackSize = new Rect(0, 0, 200, 10);

        //����б���
        public readonly Rect TrackListSize = new Rect(0, 50, 0, 0);

        public readonly Rect LeftTimeAreaSize = new Rect(220, 20, 0, 24);
        public readonly Rect RightTimeAreaSize = new Rect(0, 10, 280, 40);

        private List<BaseView> PartialViews = new List<BaseView>() { new TopToolbarView(), new TimeAreaView(), new AddTrackView() };

        public Rect WinArea { get; set; }

        public abstract string SavePath { get; }

        public bool IsPlayingSkill = false;
        public bool IsLockDragHeaderArrow = false;      //�������
        public double CutOffTime;                       //���ʱ��

        private double lastUpdateTime;
        private double runningTime;

        /// <summary>
        /// ����ʱ��
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
            //Test
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
                    SequenceData sequenceData = (SequenceData)sequenceView.Data;
                    if (RunningTime >= sequenceData.DurationTime)
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

        #region �ӿ�

        public void AddPartialView(BaseView skillView)
        {
            skillView.Init(this);
            PartialViews.Add(skillView);
        }

        public T GetPartialView<T>() where T : BaseView
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

        public void RemovePartialView<T>() where T : BaseView
        {
            for (int i = 0; i < PartialViews.Count; i++)
            {
                if (PartialViews[i] is T)
                {
                    PartialViews.RemoveAt(i);
                }
            }
        }

        //���ò���״̬
        public void SetSkillEdPlay(bool reqPlay)
        {
            if (IsPlayingSkill == reqPlay)
                return;

            IsPlayingSkill = reqPlay;
        }

        //����ʱ����
        public void LoadTimeline(string filePath)
        {
            SequenceData sequenceData = TimelineSerialize.Load(filePath);
            if (sequenceData == null)
                return;
            RemovePartialView<BaseSequenceView>();
            BaseSequenceView sequenceView = BaseTimelineView.CreateView<BaseSequenceView>();
            sequenceView.SetData(sequenceData);
            AddPartialView(sequenceView);
        }

        //����ʱ����
        public void CreateTimeline(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            TopToolbarView topToolbarView = GetPartialView<TopToolbarView>();
            if (topToolbarView.CheckHasTimeline(name))
            {
                Debug.LogError($"�������ظ�:{name}");
                return;
            }
            RemovePartialView<BaseSequenceView>();
            BaseSequenceView sequenceView = BaseTimelineView.CreateView<BaseSequenceView>();
            ((SequenceData)sequenceView.Data).Name = name;
            AddPartialView(sequenceView);
        }

        #endregion �ӿ�
    }
}