using SkillSystem.EDData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillSystem
{
    /// <summary>
    /// 基础的序列选择界面
    /// </summary>
    public class BaseSequenceSelectView : BaseSkillSelectView
    {
        public override void OnGUI()
        {
            base.OnGUI();
            SequenceData sequenceData = (SequenceData)Data;

        }
    }
}
