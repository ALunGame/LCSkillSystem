using UnityEditor;
using UnityEngine;

namespace SkillSystem
{
    //时间线区域
    public class TimeAreaView : BaseSkillView
    {
        public const float ARROW_WIDTH = 6f;

        private TimeAreaShowHelper timeAreaShow = null;
        
        private static int kRedCursorControlID = "RedCursorControlRect".GetHashCode();
        private static int kBlueCursorControlID = "BlueCursorControlRect".GetHashCode();

        //时间区域
        public Rect TimeContent     => new Rect(window.LeftTimeAreaSize.x, window.LeftTimeAreaSize.y, window.position.width - window.LeftTimeAreaSize.x, window.position.height - window.LeftTimeAreaSize.y);

        //时间标注区域
        public Rect TimeHeaderRect  => new Rect(window.LeftTimeAreaSize.x, window.LeftTimeAreaSize.y, window.position.width - window.LeftTimeAreaSize.x, window.LeftTimeAreaSize.height);

        //时间Track区域
        public Rect TimeTickRect => new Rect(window.LeftTimeAreaSize.x, window.LeftTimeAreaSize.y + window.LeftTimeAreaSize.height, window.position.width - window.LeftTimeAreaSize.x, window.position.height - window.LeftTimeAreaSize.height);

        public int frame = 30;
        private bool dragCutOffTimeArrow = false;

        public override void OnInit()
        {
            base.OnInit();
        }

        private void InitTimeArea()
        {
            if (timeAreaShow == null)
            {
                timeAreaShow = new TimeAreaShowHelper(false)
                {
                    hRangeLocked = false,
                    vRangeLocked = true,
                    margin = -2,
                    scaleWithWindow = true,
                    hSlider = true,
                    vSlider = false,
                    hBaseRangeMin = 0,
                    hBaseRangeMax = 20,
                    hRangeMin = 0,
                    hRangeMax = 100,
                    hScaleMax = 1000f,
                    rect = TimeContent,
                };
            }
        }

        public override void OnDraw()
        {
            InitTimeArea();
            GUILayout.BeginHorizontal();
            timeAreaShow.BeginViewGUI();
            timeAreaShow.TimeRuler(TimeHeaderRect, frame, true, false, 1.0f, 1);
            timeAreaShow.DrawMajorTicks(TimeTickRect, frame);
            timeAreaShow.EndViewGUI();
            GUILayout.EndHorizontal();

            DrawCursor();
        }

        //绘制滑动游标
        private void DrawCursor()
        {
            //红色指引线
            GUILayout.BeginArea(TimeContent, string.Empty/*, EditorStyles.toolbarButton*/);
            Color cl01 = GUI.color;
            GUI.color = Color.red;
            float timeToPos = TimeToPixel(window.RunningTime);
            GUI.DrawTexture(new Rect(-ARROW_WIDTH + timeToPos - TimeHeaderRect.x, 2, ARROW_WIDTH * 2f, ARROW_WIDTH * 2f * 1.82f), EDSkillLoadHelp.LoadEdStyleImg("Timecursor.png"));
            GUI.color = cl01;
            Rect lineRect = new Rect(timeToPos - TimeHeaderRect.x, 18, 1, TimeContent.height + ARROW_WIDTH);
            EditorGUI.DrawRect(lineRect, Color.red);
            GUILayout.EndArea();
        }

        public override void OnHandleEvent(Event evt)
        {
            //
            // 处理鼠标拖动游标
            //
            float timeToPos = TimeToPixel(window.CutOffTime);
            Rect cutOffRect = new Rect(-ARROW_WIDTH + timeToPos - TimeHeaderRect.x, window.position.height - ARROW_WIDTH * 2 - 18, ARROW_WIDTH * 2, ARROW_WIDTH * 2);
            int controlId = GUIUtility.GetControlID(kBlueCursorControlID, FocusType.Passive);
            int redControlId = GUIUtility.GetControlID(kRedCursorControlID, FocusType.Passive);

            if (evt.rawType == EventType.MouseUp)
            {
                if (GUIUtility.hotControl == controlId || GUIUtility.hotControl == redControlId)
                {
                    GUIUtility.hotControl = 0;
                    evt.Use();
                }
                dragCutOffTimeArrow = false;
            }
            Vector2 mousePos = new Vector2(evt.mousePosition.x - TimeTickRect.x, evt.mousePosition.y - TimeTickRect.y);
            if (!Application.isPlaying)
            {
                switch (evt.GetTypeForControl(controlId))
                {
                    case EventType.MouseDown:
                        {
                            if (cutOffRect.Contains(mousePos))
                            {
                                GUIUtility.hotControl = controlId;
                                evt.Use();
                            }
                        }
                        break;
                    case EventType.MouseDrag:
                        {
                            if (GUIUtility.hotControl == controlId)
                            {
                                Vector2 vec = new Vector2(evt.mousePosition.x, evt.mousePosition.y);
                                double fTime = GetSnappedTimeAtMousePosition(vec);
                                if (fTime <= 0)
                                    fTime = 0;
                                window.CutOffTime = fTime;
                                dragCutOffTimeArrow = true;
                            }
                        }
                        break;
                    default: break;
                }
            }

            //
            // Drag cut off time guide line
            //
            // Mouse for time guide line
            evt = Event.current;
            mousePos = evt.mousePosition;

            if (!Application.isPlaying)
            {
                switch (evt.GetTypeForControl(redControlId))
                {
                    case EventType.MouseDown:
                        {
                            if (TimeHeaderRect.Contains(mousePos))
                            {
                                GUIUtility.hotControl = redControlId;
                                evt.Use();
                                double fTime = GetSnappedTimeAtMousePosition(mousePos);
                                if (fTime <= 0)
                                    fTime = 0.0;
                                window.RunningTime = fTime;
                            }
                        }
                        break;
                    case EventType.MouseDrag:
                        {
                            if (GUIUtility.hotControl == redControlId)
                            {
                                if (!window.IsLockDragHeaderArrow)
                                {
                                    double fTime = GetSnappedTimeAtMousePosition(mousePos);
                                    if (fTime <= 0)
                                        fTime = 0.0;
                                    window.RunningTime = fTime;
                                }
                            }
                        }
                        break;
                    default: break;
                }
            }
        }

        public float TimeToPixel(double time)
        {
            return timeAreaShow.TimeToPixel((float)time, TimeContent);
        }

        public float PiexlToTime(double piexl)
        {
            return timeAreaShow.PixelToTime((float)piexl, TimeContent);
        }

        public double SnapToFrameIfRequired(double time)
        {
            double result;
            result = time;
            return result;
        }

        public double GetSnappedTimeAtMousePosition(Vector2 mousePos)
        {
            return SnapToFrameIfRequired((double)this.ScreenSpacePixelToTimeAreaTime(mousePos.x));
        }

        public float ScreenSpacePixelToTimeAreaTime(float p)
        {
            p -= TimeContent.x;
            return TrackSpacePixelToTimeAreaTime(p);
        }

        public float TrackSpacePixelToTimeAreaTime(float p)
        {
            p -= timeAreaShow.translation.x;
            float result;
            if (timeAreaShow.scale.x > 0f)
            {
                result = p / timeAreaShow.scale.x;
            }
            else
            {
                result = p;
            }
            return result;
        }
    }
}
