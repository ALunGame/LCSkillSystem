using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkillSystem
{
    public abstract class BaseSkillView   
    {
        protected SkillEditorWindow window;

        protected BaseSkillSelectView selectView;

        public void Init(SkillEditorWindow editorWindow, BaseSkillSelectView selectView = null)
        {
            this.window = editorWindow;
            this.selectView = selectView;
            OnInit();
        }

        public virtual void OnInit()
        {

        }

        public abstract void OnDraw();

        public virtual void OnDestroy()
        {

        }

        public virtual void OnHandleEvent(Event evt)
        {

        }

        public void ShowSelectView()
        {
            window.InspectorView.SelectSkillObject(selectView);
            Selection.activeObject = window.InspectorView;
        }
    } 
}
