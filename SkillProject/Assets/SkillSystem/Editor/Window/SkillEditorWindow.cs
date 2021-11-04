using System.Collections;
using System.Collections.Generic;
using Timeline;
using UnityEditor;
using UnityEngine;

namespace LCSkill
{
    public class SkillEditorWindow : TimelineEditorWindow
    {
        [MenuItem("Skill/¾ŽÝ‹", false, 1)]
        public static void ShowWindow()
        {
            SkillEditorWindow window = GetWindow<SkillEditorWindow>(false, "¼¼ÄÜ¾ŽÝ‹", true);
            window.minSize = new Vector2(400, 300);
        }
    }
}