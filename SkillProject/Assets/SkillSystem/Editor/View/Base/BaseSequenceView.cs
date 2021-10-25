using SkillSystem.EDData;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

namespace SkillSystem
{
    public enum EDSequencePlayMode
    {
        EditorPause,
        EditorRun,
    }

    /// <summary>
    /// 序列视图（一个序列包含多个轨道）
    /// </summary>
    public class BaseSequenceView : BaseSkillView
    {
        private static Color ColorBg = new Color(0.66f, 0.66f, 0.66f, 0.1f);
        //序列最后标识线
        private static Color ColorEndLine = new Color(0.8f, 0.1f, 0, 0.5f);

        public EDSequencePlayMode PlayMode;
        public List<BaseTrackView> Tracklist = new List<BaseTrackView>();

        private GameObject SequenceRootGo;
        public SequenceData SequenceData;
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
            //Test
            SequenceData = new SequenceData();
            for (int i = 0; i < 10; i++)
            {
                BaseTrackView trackView = new BaseTrackView();
                trackView.Init(window);
                trackView.Sequence = this;
                trackView.TrackIndex = i;
                Tracklist.Add(trackView);
            }
        }

        public override void OnDraw()
        {
            //显示
            SequenceViewRect        = window.TrackListSize;
            SequenceViewRect.height = window.position.height - window.TrackListSize.y;
            SequenceViewRect.width  = window.position.width - window.TrackListSize.x;

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
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            float x = timeAreaView.TimeToPixel(SequenceData.DurationTime);
            if (SequenceData.DurationTime > 1e-1 && x > 200)
            {
                Rect rec = new Rect(x, timeAreaView.TimeContent.y, 1,
                    TracksBottommY - timeAreaView.TimeContent.y - 2);
                EditorGUI.DrawRect(rec, ColorEndLine);
            }
        }

        public override void OnHandleEvent(Event evt)
        {
            for (int i = 0; i < Tracklist.Count; i++)
            {
                Tracklist[i].OnHandleEvent(evt);
            }
        }

        //同步数据
        private void SyncSequenceData()
        {
            SequenceData.DurationTime = CalcSequenceDurationTime();
        }

        public void AddTrackView(BaseTrackView track)
        {
            //AddTrack(track, hierachy.Count, arg);
        }

        public float CalcSequenceDurationTime()
        {
            float dur = 0;
            for (int i = 0; i < Tracklist.Count; i++)
            {
                BaseTrackView trackView = Tracklist[i];
                trackView.Cliplist.ForEach((clip) =>
                {
                    if (clip.ClipData.EndTime > dur) dur = clip.ClipData.EndTime;
                });
            }
            return dur;
        }
    }
}
