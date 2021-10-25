using SkillSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillEditorWindow : EditorWindow
{
    [MenuItem("Skill/編輯", false, 1)]
    public static void ShowWindow()
    {
        SkillEditorWindow window = GetWindow<SkillEditorWindow>(false, "技能編輯", true);
        window.minSize = new Vector2(400, 300);
    }

    public readonly Rect ToolbarSize        = new Rect(0, 0, 0, 10);
    public readonly Rect AddTrackSize       = new Rect(0, 0, 200, 10);

    public readonly Rect TrackListSize      = new Rect(0, 50, 0, 0);

    public readonly Rect LeftTimeAreaSize   = new Rect(220, 20, 0, 24);
    public readonly Rect RightTimeAreaSize  = new Rect(0, 10, 280, 40);

    private List<BaseSkillView> PartialViews = new List<BaseSkillView>() { new TopToolbarView(), new TimeAreaView(), new AddTrackView(),new BaseSequenceView() };

    public Rect WinArea { get; set; }

    public bool IsPlayingSkill = false;
    public bool IsLockDragHeaderArrow = false;      //标尺锁定
    
    public double LastUpdateTime;
    public double CutOffTime;
    public double RunningTime = 0;

    public SkillObjectInspector InspectorView;

    private void OnEnable()
    {
        InspectorView = SkillObjectInspector.CreateInspector();
        for (int i = 0; i < PartialViews.Count; i++)
        {
            PartialViews[i].Init(this);
        }
        EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
        LastUpdateTime = (float)EditorApplication.timeSinceStartup;
    }

    private void OnGUI()
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

    private void OnDestroy()
    {
        for (int i = 0; i < PartialViews.Count; i++)
        {
            PartialViews[i].OnDestroy();
        }
        EditorApplication.update = (EditorApplication.CallbackFunction)System.Delegate.Remove(EditorApplication.update, new EditorApplication.CallbackFunction(OnEditorUpdate));
    }

    public void SetSkillEdPlay(bool reqPlay)
    {
        if (IsPlayingSkill==reqPlay)
            return;

        IsPlayingSkill = reqPlay;
    }

    private void OnEditorUpdate()
    {
        if (!Application.isPlaying && IsPlayingSkill)
        {
            float delta = 1.0f / 30;
            double fTime = (float)EditorApplication.timeSinceStartup - LastUpdateTime;

            if (fTime > delta)
            {
                RunningTime += delta;
                BaseSequenceView sequenceView = GetPartialView<BaseSequenceView>();

                if (RunningTime >= sequenceView.SequenceData.DurationTime)
                {
                    RunningTime = 0;
                    //IsPlayingSkill = false;
                }
                LastUpdateTime = (float)EditorApplication.timeSinceStartup;
            }
        }
        Repaint();
    }

    public T GetPartialView<T>() where T : BaseSkillView
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
}
