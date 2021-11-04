using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using XPToolchains.Core;
using UnityObject = UnityEngine.Object;

namespace XPToolchains.Extension
{
    /// <summary>
    /// Inspector面板扩展
    /// </summary>
    public static partial class EditorInspectorExtension
    {
        /// <summary>
        /// 在Inspector面板上绘制一个对象
        /// </summary>
        public static void DrawObjectInInspector(object _targetObject, UnityObject _unityOwner = null)
        {
            if (_targetObject is UnityObject)
            {
                Selection.activeObject = _targetObject as UnityObject;
            }
            else
            {
                Selection.activeObject = ObjectInspector.Instance;
                ObjectInspector.Instance.Init(_targetObject, _unityOwner);
            }
        }

        /// <summary>
        /// 在Inspector面板上绘制一个对象
        /// </summary>
        public static void DrawObjectInInspector(string _title, object _targetObject, UnityObject _unityOwner = null)
        {
            DrawObjectInInspector(_targetObject, _unityOwner);
            ObjectInspector.Instance.name = _title;
        }
    }
}