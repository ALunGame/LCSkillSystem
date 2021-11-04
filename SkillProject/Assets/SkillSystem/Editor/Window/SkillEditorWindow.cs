using System.Collections;
using System.Collections.Generic;
using Timeline;
using UnityEditor;
using UnityEngine;

namespace LCSkill
{
    public class SkillEditorWindow : TimelineEditorWindow
    {
        [MenuItem("Skill/��݋", false, 1)]
        public static void ShowWindow()
        {
            SkillEditorWindow window = GetWindow<SkillEditorWindow>(false, "���ܾ�݋", true);
            window.minSize = new Vector2(400, 300);
        }
    }
}