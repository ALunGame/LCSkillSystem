using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityObject = UnityEngine.Object;

#if UNITY_EDITOR

using UnityEditor.SceneManagement;
using UnityEditor;

#endif

namespace XPToolchains
{
    /// <summary>
    /// 需要序列化的资源
    /// </summary>
    public class UnityObjectAsset
    {
        [Json.JsonIgnore]
        public UnityObject Obj;

        public string ObjPath;

#if UNITY_EDITOR

        public UnityObject GetObj(Type objType, bool sceneObj)
        {
            if (string.IsNullOrEmpty(ObjPath))
                return null;
            if (sceneObj)
            {
                Scene scene = EditorSceneManager.GetActiveScene();
                GameObject[] rootGos = scene.GetRootGameObjects();
                for (int i = 0; i < rootGos.Length; i++)
                {
                    GameObject rootGo = rootGos[i];
                    if (ObjPath.Contains(rootGo.name))
                    {
                        Transform targetTrans = null;
                        if (!ObjPath.Contains("/"))
                            targetTrans = rootGo.transform;
                        else
                        {
                            int index = ObjPath.IndexOf('/') + 1;
                            string leftPath = ObjPath.Substring(index, ObjPath.Length - index);
                            targetTrans = rootGo.transform.Find(leftPath);
                        }

                        if (targetTrans != null)
                        {
                            if (objType == typeof(GameObject))
                                return targetTrans.gameObject;
                            else

                                return targetTrans.GetComponent(objType);
                        }
                    }
                }
            }
            else
            {
                return AssetDatabase.LoadAssetAtPath(ObjPath, objType);
            }
            return null;
        }

#endif
    }
}