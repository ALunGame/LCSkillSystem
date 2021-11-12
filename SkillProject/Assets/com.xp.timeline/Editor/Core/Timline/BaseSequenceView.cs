using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

namespace Timeline.View
{
    public enum EDSequencePlayMode
    {
        EditorPause,
        EditorRun,
    }

    /// <summary>
    /// 序列视图（一个序列包含多个轨道）
    /// </summary>
    [TimelineView(typeof(SequenceData))]
    public class BaseSequenceView : BaseTimelineView
    {
        private static Color ColorBg = new Color(0.66f, 0.66f, 0.66f, 0.1f);

        //序列最后标识线
        private static Color ColorEndLine = new Color(0.8f, 0.1f, 0, 0.5f);

        public EDSequencePlayMode PlayMode;
        public List<BaseTrackView> Tracklist = new List<BaseTrackView>();

        private GameObject SequenceRootGo;
        private float PlayTimer;

        public Rect SequenceViewRect;

        private Vector2 TracklistScroll;

        //轨道最下方坐标
        public float TracksBottommY
        {
            get
            {
                if (Tracklist != null && Tracklist.Count > 0)
                {
                    BaseTrackView track = Tracklist.Last();
                    return track.TrackViewRect.y + track.TrackViewRect.height;
                }
                return window.TrackListSize.y;
            }
        }

        public override void OnInit()
        {
            SequenceData data = (SequenceData)Data;
            for (int i = 0; i < data.Tracks.Count; i++)
            {
                BaseTrackView trackView = CreateView(data.Tracks[i]) as BaseTrackView;
                AddTrackView(trackView, false);
            }
        }

        public override void OnDraw()
        {
            //显示
            SequenceViewRect = window.TrackListSize;
            SequenceViewRect.height = window.position.height - window.TrackListSize.y;
            SequenceViewRect.width = window.position.width - window.TrackListSize.x;

            //绘制背景
            EditorGUI.DrawRect(SequenceViewRect, ColorBg);

            //绘制所有轨道
            EDLayout.CreateVertical("", SequenceViewRect.width, SequenceViewRect.height, () =>
            {
                if (Tracklist != null && Tracklist.Count > 0)
                {
                    for (int i = 0; i < Tracklist.Count; i++)
                    {
                        Tracklist[i].OnDraw();
                        GUILayout.Space(BaseTrackView.TrackOffset);
                    }
                }
            });

            //同步数据
            SyncSequenceData();

            //绘制结束标识线
            DrawEndLine();
        }

        //绘制结束标识线
        private void DrawEndLine()
        {
            SequenceData sequenceData = (SequenceData)Data;
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            float x = timeAreaView.TimeToPixel(sequenceData.DurationTime);
            if (sequenceData.DurationTime > 1e-1 && x > 200)
            {
                Rect rec = new Rect(x, timeAreaView.TimeContent.y, 1,
                    TracksBottommY - timeAreaView.TimeContent.y - 2);
                EditorGUI.DrawRect(rec, ColorEndLine);
            }
        }

        public override void OnHandleEvent(Event evt)
        {
            Vector2 mousePos = evt.mousePosition;
            switch (evt.type)
            {
                case EventType.MouseDown:
                    if (SequenceViewRect.Contains(mousePos))
                        OnSelect();
                    break;

                default:
                    break;
            }
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].OnHandleEvent(evt);
            }
        }

        public override void OnRunningTimeChange(double runningTime)
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].OnRunningTimeChange(runningTime);
            }
        }

        public virtual void OnSelect()
        {
            XPToolchains.Extension.EditorInspectorExtension.DrawObjectInInspector(Data);
        }

        //同步数据
        private void SyncSequenceData()
        {
            SequenceData sequenceData = (SequenceData)Data;
            sequenceData.DurationTime = CalcSequenceDurationTime();
        }

        private void RefreshTrackIndex()
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].TrackIndex = i;
            }
        }

        private float CalcSequenceDurationTime()
        {
            float dur = 0;
            for (int i = 0; i < Tracklist.Count; i++)
            {
                BaseTrackView trackView = Tracklist[i];
                trackView.Cliplist.ForEach((clip) =>
                {
                    ClipData clipData = (ClipData)clip.Data;
                    if (clipData.EndTime > dur)
                        dur = clipData.EndTime;
                });
            }
            return dur;
        }

        #region 接口

        public void AddTrackView(BaseTrackView track, bool savaData = true)
        {
            track.Init(window);
            track.Sequence = this;
            Tracklist.Add(track);
            RefreshTrackIndex();
            if (savaData)
            {
                SequenceData data = (SequenceData)Data;
                data.Tracks.Add((TrackData)track.Data);
            }
        }

        public void RemoveTrackView(BaseTrackView track)
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                if (Tracklist[i].Equals(track))
                {
                    Tracklist.RemoveAt(i);
                    break;
                }
            }
            SequenceData data = (SequenceData)Data;
            data.Tracks.Remove((TrackData)track.Data);
            RefreshTrackIndex();
        }

        #endregion 接口
    }
}