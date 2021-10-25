using UnityEditor;
using UnityEngine;

namespace SkillSystem
{
    public class SkillObjectInspector : ScriptableObject
    {
        public static SkillObjectInspector CreateInspector()
        {
            var inspector       = ScriptableObject.CreateInstance<SkillObjectInspector>();
            inspector.name      = "SkillObject";
            inspector.hideFlags = HideFlags.HideAndDontSave ^ HideFlags.NotEditable;
            return inspector;
        }

        public BaseSkillSelectView SkillObjectView;

        public void SelectSkillObject(BaseSkillSelectView skillObjectView)
        {
            SkillObjectView = skillObjectView;
        }
    }

    [CustomEditor(typeof(SkillObjectInspector))]
    public class SkillObjectInspectorEditor : Editor
    {
        private SkillObjectInspector objectInspector;

        private void OnEnable()
        {
            objectInspector = target as SkillObjectInspector;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (objectInspector.SkillObjectView!=null)
            {
                objectInspector.SkillObjectView.OnGUI();
            }
        }
    }
}
