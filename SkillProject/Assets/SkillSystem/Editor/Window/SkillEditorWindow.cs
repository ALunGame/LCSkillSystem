using Timeline;
using UnityEditor;
using UnityEngine;

namespace LCSkill
{
    public class SkillEditorWindow : TimelineEditorWindow
    {
        public override string SavePath => "Assets/SkillSystem/EDData/";

        [MenuItem("Skill/��݋", false, 1)]
        public static void ShowWindow()
        {
            SkillEditorWindow window = GetWindow<SkillEditorWindow>(true, "���ܾ�݋", true);
            window.minSize = new Vector2(400, 300);
        }
    }
}