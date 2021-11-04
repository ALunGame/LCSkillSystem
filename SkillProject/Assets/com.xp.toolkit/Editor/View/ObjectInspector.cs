using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityObject = UnityEngine.Object;

namespace XPToolchains.Core
{
    /// <summary>
    /// 绘制在Inspector面板的中介
    /// </summary>
    public class ObjectInspector : ScriptableSingleton<ObjectInspector>
    {
        [SerializeField]
        object targetObject;

        public Action onTargetObjectChanged;

        public object TargetObject
        {
            get { return targetObject; }
            private set
            {
                if (targetObject == null || !targetObject.Equals(value))
                {
                    targetObject = value;
                    onTargetObjectChanged?.Invoke();
                }
            }
        }
        public UnityObject UnityOwner { get; private set; }

        public void Init(object _targetObject, UnityObject _unityOwner)
        {
            UnityOwner = _unityOwner;
            TargetObject = _targetObject;
        }
    }

    [CustomEditor(typeof(ObjectInspector))]
    public class ObjectInspectorEditor : Editor
    {
        ObjectInspectorDrawer objectEditor;

        public UnityObject UnityOwner { get; set; }

        public ObjectInspector T_Target { get { return target as ObjectInspector; } }

        private void OnEnable()
        {
            UnityOwner = T_Target.UnityOwner;

            OnEnable(T_Target.TargetObject);
            T_Target.onTargetObjectChanged = () =>
            {
                OnEnable(T_Target.TargetObject);
                Repaint();
            };

            void OnEnable(object _targetObject)
            {
                objectEditor = ObjectInspectorDrawer.CreateEditor(_targetObject, UnityOwner, this);
                if (objectEditor != null)
                {
                    string title = objectEditor.GetTitle();
                    if (!string.IsNullOrEmpty(title))
                        target.name = title;
                    objectEditor.OnEnable();
                }
            }
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
            if (objectEditor != null)
                objectEditor.OnHeaderGUI();
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement v = null;
            if (objectEditor != null)
                v = objectEditor.CreateInspectorGUI();
            if (v == null)
                v = base.CreateInspectorGUI();
            return v;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            if (objectEditor != null)
                objectEditor.OnInspectorGUI();
        }

        public override void DrawPreview(Rect previewArea)
        {
            base.DrawPreview(previewArea);
            if (objectEditor != null)
                objectEditor.DrawPreview(previewArea);
        }

        public override bool HasPreviewGUI()
        {
            if (objectEditor != null)
                return objectEditor.HasPreviewGUI();
            return base.HasPreviewGUI();
        }

        public override void OnPreviewSettings()
        {
            base.OnPreviewSettings();
            if (objectEditor != null)
                objectEditor.OnPreviewSettings();
        }

        public override GUIContent GetPreviewTitle()
        {
            GUIContent title = null;
            if (objectEditor != null)
                title = objectEditor.GetPreviewTitle();
            if (title == null)
                title = base.GetPreviewTitle();
            return title;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            base.OnPreviewGUI(r, background);
            if (objectEditor != null)
                objectEditor.OnPreviewGUI(r, background);
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            base.OnInteractivePreviewGUI(r, background);
            if (objectEditor != null)
                objectEditor.OnInteractivePreviewGUI(r, background);
        }

        private void OnSceneGUI()
        {
            if (objectEditor != null)
                objectEditor.OnSceneGUI();
        }

        private void OnValidate()
        {
            if (objectEditor != null)
                objectEditor.OnValidate();
        }

        private void OnDisable()
        {
            if (objectEditor != null)
                objectEditor.OnDisable();
        }
    }
}
