using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeline
{
    /// <summary>
    /// 显隐
    /// </summary>
    public class TL_ActiveClipData : ClipData
    {
        public bool isActive;
    }

    /// <summary>
    /// 通用动画
    /// </summary>
    public class TL_AnimClipData : ClipData
    {
        public string animName;
    }
}