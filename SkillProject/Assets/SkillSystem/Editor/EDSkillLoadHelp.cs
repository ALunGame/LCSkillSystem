using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace SkillSystem
{
    public class EDSkillLoadHelp
    {
        private static readonly string EditImgPath = @"Assets/SkillSystem/Editor/Images/";


        public static Texture2D LoadEdStyleImg(string imgName)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(EditImgPath + imgName);
        }
    }
}
