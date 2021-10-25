using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    public class SkillData
    {
        public int SkillId;
        public BaseSkillSequenceData SkillSequenceData = new BaseSkillSequenceData();
    }

    public class BaseSkillSequenceData
    {
        public float Duration;
        public List<BaseSkillTrackData> TrackDatas = new List<BaseSkillTrackData>();
    }

    public class BaseSkillTrackData
    {
        public List<BaseSkillClipData> ClipDatas = new List<BaseSkillClipData>();
    }

    public class BaseSkillClipData
    {
        //��ʼ
        public float Start;
        //����
        public float Duration;
    } 
}
