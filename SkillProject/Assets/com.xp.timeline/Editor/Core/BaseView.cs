using System.Collections;
using UnityEngine;

namespace Timeline
{
    public class BaseView
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

        public virtual void OnDraw()
        {
        }

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