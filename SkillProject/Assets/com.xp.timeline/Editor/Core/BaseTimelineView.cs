using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    public abstract class BaseTimelineView
    {
        protected TimelineEditorWindow window;

        public void Init(TimelineEditorWindow editorWindow)
        {
            this.window = editorWindow;
            OnInit();
        }

        public void SyncRunningTime(double runningTime)
        {
            OnRunningTimeChange(runningTime);
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

        public virtual void OnRunningTimeChange(double runningTime)
        {

        }
    } 
}
