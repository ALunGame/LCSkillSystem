using SkillSystem;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

/// <summary>
/// 顶部工具栏
/// </summary>
public class TopToolbarView : BaseSkillView
{
    #region 播放操作UI元素
    private GUIContent PlayContent;

    private GUIContent GotoBeginingContent;

    private GUIContent GotoEndContent;

    private GUIContent NextFrameContent;

    private GUIContent PreviousFrameContent;
    #endregion

    #region 按钮列表

    private GUIContent NewContent;
    private GUIContent OpenContent;
    private GUIContent SaveContent;
    private GUIContent RefreshContent;
    private GUIContent InpectContent;

    #endregion

    private const float HorWidth = 200;

    public override void OnInit()
    {
        PlayContent             = EditorGUIUtility.TrIconContent("Animation.Play", "Play the timeline");
        GotoBeginingContent     = EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the timeline");
        GotoEndContent          = EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the timeline");
        NextFrameContent        = EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to the next frame");
        PreviousFrameContent    = EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to the previous frame");

        NewContent              = new GUIContent(EDSkillLoadHelp.LoadEdStyleImg("btn_editor_new.png"), "新建.");
        OpenContent             = new GUIContent(EDSkillLoadHelp.LoadEdStyleImg("btn_editor_open.png"), "打开.");
        SaveContent             = new GUIContent(EDSkillLoadHelp.LoadEdStyleImg("btn_editor_save.png"), "保存.");
        RefreshContent          = new GUIContent(EDSkillLoadHelp.LoadEdStyleImg("btn_editor_refresh.png"), "刷新.");
        InpectContent           = new GUIContent(EDSkillLoadHelp.LoadEdStyleImg("inspect.png"), "inspect显示.");
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

    //绘制播放操作
    private void DrawPlayOperate()
    {
        //最前
        if (GUILayout.Button(GotoBeginingContent, EditorStyles.toolbarButton))
        {
        }
        //上一帧
        if (GUILayout.Button(PreviousFrameContent, EditorStyles.toolbarButton))
        {
        }
        //播放
        if (GUILayout.Button(PlayContent, EditorStyles.toolbarButton))
        {
            window.SetSkillEdPlay(!window.IsPlayingSkill);
        }
        //下一帧
        if (GUILayout.Button(NextFrameContent, EditorStyles.toolbarButton))
        {
        }
        //最后
        if (GUILayout.Button(GotoEndContent, EditorStyles.toolbarButton))
        {
        }
    }

    //绘制技能时间
    private void DrawSkillLineTime()
    {
        EditorGUI.BeginChangeCheck();

        window.RunningTime = EditorGUILayout.DoubleField(window.RunningTime,
            EditorStyles.toolbarTextField);

        if (EditorGUI.EndChangeCheck())
        {
        }
    }

    //按钮列表
    private void DrawBtnList()
    {
        if (GUILayout.Button(NewContent, EditorStyles.toolbarButton))
        {
            GUIUtility.ExitGUI();
        }

        if (GUILayout.Button(OpenContent, EditorStyles.toolbarButton))
        {
            GUIUtility.ExitGUI();
        }

        if (GUILayout.Button(SaveContent, EditorStyles.toolbarButton))
        {
            GUIUtility.ExitGUI();
        }

        if (GUILayout.Button(RefreshContent, EditorStyles.toolbarButton, GUILayout.MaxWidth(24)))
        {
        }

        GUILayout.Space(4);
        if (GUILayout.Button(InpectContent, EditorStyles.toolbarButton, GUILayout.MaxWidth(30)))
        {
            //SeqenceInspector.ShowWindow();
        }
    }
}
