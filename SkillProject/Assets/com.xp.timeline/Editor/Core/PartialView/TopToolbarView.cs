using System;
using Timeline;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

/// <summary>
/// ����������
/// </summary>
public class TopToolbarView : BaseView
{
    #region ���Ų���UIԪ��

    private GUIContent PlayContent;

    private GUIContent GotoBeginingContent;

    private GUIContent GotoEndContent;

    private GUIContent NextFrameContent;

    private GUIContent PreviousFrameContent;

    #endregion ���Ų���UIԪ��

    #region ��ť�б�

    private GUIContent NewContent;
    private GUIContent OpenContent;
    private GUIContent SaveContent;

    #endregion ��ť�б�

    private const float HorWidth = 200;

    public Action OnClickNewFileFunc = null;
    public Action OnClickOpenFileFunc = null;
    public Action OnClickSaveFileFunc = null;

    public override void OnInit()
    {
        PlayContent = EditorGUIUtility.TrIconContent("Animation.Play", "Play the timeline");
        GotoBeginingContent = EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the timeline");
        GotoEndContent = EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the timeline");
        NextFrameContent = EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to the next frame");
        PreviousFrameContent = EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to the previous frame");

        NewContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_new.png"), "�½�.");
        OpenContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_open.png"), "��.");
        SaveContent = new GUIContent(TimelineStyle.LoadEdStyleImg("btn_editor_save.png"), "����.");
    }

    public override void OnDraw()
    {
        GUILayout.BeginHorizontal();

        EDLayout.CreateHorizontal("", HorWidth, window.ToolbarSize.height, () =>
        {
            DrawPlayOperate();
            GUILayout.FlexibleSpace();

            DrawSkillLineTime();
        });

        GUILayout.FlexibleSpace();
        GUILayout.Label("Test");
        GUILayout.FlexibleSpace();

        EDLayout.CreateHorizontal("", HorWidth, window.ToolbarSize.height, () =>
        {
            DrawBtnList();
        });

        GUILayout.EndHorizontal();
    }

    //���Ʋ��Ų���
    private void DrawPlayOperate()
    {
        //��ǰ
        if (GUILayout.Button(GotoBeginingContent, EditorStyles.toolbarButton))
        {
        }
        //��һ֡
        if (GUILayout.Button(PreviousFrameContent, EditorStyles.toolbarButton))
        {
        }
        //����
        if (GUILayout.Button(PlayContent, EditorStyles.toolbarButton))
        {
            window.SetSkillEdPlay(!window.IsPlayingSkill);
        }
        //��һ֡
        if (GUILayout.Button(NextFrameContent, EditorStyles.toolbarButton))
        {
        }
        //���
        if (GUILayout.Button(GotoEndContent, EditorStyles.toolbarButton))
        {
        }
    }

    //���Ƽ���ʱ��
    private void DrawSkillLineTime()
    {
        EditorGUI.BeginChangeCheck();

        window.RunningTime = EditorGUILayout.DoubleField(window.RunningTime,
            EditorStyles.toolbarTextField);

        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    //��ť�б�
    private void DrawBtnList()
    {
        if (GUILayout.Button(NewContent, EditorStyles.toolbarButton))
        {
            OnClickNewFileFunc?.Invoke();
            GUIUtility.ExitGUI();
        }

        if (GUILayout.Button(OpenContent, EditorStyles.toolbarButton))
        {
            OnClickOpenFileFunc?.Invoke();
            GUIUtility.ExitGUI();
        }

        if (GUILayout.Button(SaveContent, EditorStyles.toolbarButton))
        {
            OnClickSaveFileFunc?.Invoke();
            GUIUtility.ExitGUI();
        }
    }
}