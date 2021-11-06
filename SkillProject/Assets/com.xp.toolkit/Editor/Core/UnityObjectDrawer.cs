using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XPToolchains.Help;
using UnityObject = UnityEngine.Object;

namespace XPToolchains.Core
{
    [CustomObjectDrawer(typeof(UnityObjectAsset))]
    public class UnityObjectDrawer : ObjectDrawer
    {
        private GameObject GetRootGo(GameObject gameObject)
        {
            if (gameObject == null || gameObject.transform.parent == null)
                return gameObject;
            return GetRootGo(gameObject.transform.parent.gameObject);
        }

        private string GetPathParentToChild(Transform child)
        {
            if (child == null)
            {
                return "";
            }

            Transform selectChild = child.transform;
            string result = "";
            if (selectChild != null)
            {
                result = selectChild.name;
                while (selectChild.parent != null)
                {
                    selectChild = selectChild.parent;
                    result = string.Format("{0}/{1}", selectChild.name, result);
                }
            }
            return result;
        }

        private void GetAsset(UnityObjectAsset unityObject, Type objType, bool sceneObj)
        {
            if (unityObject == null || string.IsNullOrEmpty(unityObject.ObjPath))
                return;
            unityObject.Obj = unityObject.GetObj(objType, sceneObj);
        }

        private void UpdateAssetPath(UnityObjectAsset unityObject, Type objType, bool sceneObj)
        {
            if (unityObject == null || unityObject.Obj == null)
            {
                unityObject.ObjPath = "";
                return;
            }

            if (sceneObj)
            {
                GameObject objGo = null;
                if (objType == typeof(GameObject))
                    objGo = (GameObject)unityObject.Obj;
                else if (unityObject.Obj is Component)
                    objGo = ((Component)unityObject.Obj).gameObject;
                unityObject.ObjPath = GetPathParentToChild(objGo.transform);
            }
            else
            {
                unityObject.ObjPath = AssetDatabase.GetAssetPath(unityObject.Obj);
            }
        }

        public override void OnGUI(Rect _position, GUIContent _label)
        {
            if (Target == null)
            {
                Target = new UnityObjectAsset();
            }

            UnityObjectAsset unityObject = Target as UnityObjectAsset;
            if (AttributeHelper.TryGetFieldInfoAttribute(FieldInfo, out UnityAssetTypeAttribute assetTypeAttribute))
            {
                GetAsset(unityObject, assetTypeAttribute.ObjType, assetTypeAttribute.SceneObj);
                UnityObject tmpObj = unityObject.Obj;
                tmpObj = EditorGUILayout.ObjectField(_label, tmpObj, assetTypeAttribute.ObjType, assetTypeAttribute.SceneObj);
                if (tmpObj != null && !tmpObj.Equals(unityObject.Obj))
                {
                    unityObject.Obj = tmpObj;
                    UpdateAssetPath(unityObject, assetTypeAttribute.ObjType, assetTypeAttribute.SceneObj);
                }
            }
            else
            {
                _label.text = "需要声明UnityAssetTypeAttribute属性";
                GUI.Label(_position, _label);
            }
        }
    }
}