using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XPToolchains.Extension;
using XPToolchains.Help;
using UnityObject = UnityEngine.Object;

namespace XPToolchains.Core
{
    /// <summary>
    /// 自定义类型绘制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class CustomObjectDrawerAttribute : Attribute
    {
        public Type targetType;

        public CustomObjectDrawerAttribute(Type _targetType) { targetType = _targetType; }
    }

    /// <summary>
    /// 自定义字段绘制
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class CustomFieldDrawerAttribute : Attribute
    {
        Type type;

        public Type Type { get { return type; } }

        public CustomFieldDrawerAttribute(Type _type) { type = _type; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
    public abstract class FieldAttribute : Attribute { }

    /// <summary>
    /// EditorGUI面板绘制器
    /// </summary>
    public class ObjectDrawer
    {
        #region Static

        static Dictionary<Type, Type> ObjectEditorTypeCache;

        static ObjectDrawer()
        {
            BuildCache();
        }

        static void BuildCache()
        {
            ObjectEditorTypeCache = new Dictionary<Type, Type>();

            foreach (var type in TypeCache.GetTypesDerivedFrom<ObjectDrawer>())
            {
                foreach (var att in AttributeHelper.GetTypeAttributes(type, false))
                {
                    if (att is CustomObjectDrawerAttribute sAtt)
                        ObjectEditorTypeCache[sAtt.targetType] = type;
                }
            }
        }

        public static bool HasCustomEditor()
        {
            return false;
        }

        static ObjectDrawer InternalCreateEditor(object _targetObject)
        {
            if (_targetObject == null) return null;

            return Activator.CreateInstance(GetEditorType(_targetObject.GetType()), true) as ObjectDrawer;
        }

        public static Type GetEditorType(Type _objectType)
        {
            if (ObjectEditorTypeCache.TryGetValue(_objectType, out Type editorType))
                return editorType;
            if (_objectType.BaseType != null)
                return GetEditorType(_objectType.BaseType);
            else
                return typeof(ObjectDrawer);
        }

        public static ObjectDrawer CreateEditor(object _targetObject)
        {
            ObjectDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject);
            return objectEditor;
        }

        #endregion

        protected ObjectDrawer() { }

        public FieldInfo FieldInfo { get; set; }
        public FieldAttribute Attribute { get; set; }
        public object Target { get; set; }
        protected IReadOnlyList<FieldInfo> Fields { get; private set; }

        void Init(object _target)
        {
            Target = _target;
            Fields = ReflectionHelper.GetFieldInfos(Target.GetType()).Where(field => EditorGUILayoutExtension.CanDraw(field)).ToList();
        }

        void Init(object _target, FieldInfo _field)
        {
            Target = _target;
            Fields = ReflectionHelper.GetFieldInfos(Target.GetType()).Where(field => EditorGUILayoutExtension.CanDraw(field)).ToList();
        }


        public virtual void OnGUI(Rect _position, GUIContent _label)
        {
            GUI.Label(_position, _label);
        }

        public virtual float GetHeight()
        {
            return 20;
        }
    }
}
