using System;

namespace XPToolchains
{
    /// <summary>
    /// 声明字段自定义绘制
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FieldDrawerAttribute : Attribute { }

    /// <summary>
    /// Unity内部资源
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UnityAssetTypeAttribute : Attribute
    {
        private Type objType;

        public Type ObjType { get { return objType; } }

        private bool sceneObj;

        public bool SceneObj { get { return sceneObj; } }

        /// <summary>
        /// Unity内部资源
        /// </summary>
        public UnityAssetTypeAttribute(Type _type, bool _sceneObj)
        {
            objType = _type;
            sceneObj = _sceneObj;
        }
    }
}