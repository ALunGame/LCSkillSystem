using System;
using UnityEngine;

namespace LCSkill
{
    public class SkillServer
    {
        public float CurrTime
        {
            get
            {
                return Time.realtimeSinceStartup;
            }
        }

        //public SkillSequence<BaseSkillSequenceData> CreateSkillSequence(object param)
        //{
        //    Type type   = typeof(SkillSequence<>);
        //    type        = type.MakeGenericType(param.GetType());
        //    object o    = Activator.CreateInstance(type, param);
        //    SkillSequence<BaseSkillSequenceData> skillObj = o as SkillSequence<BaseSkillSequenceData>;
        //    return skillObj;
        //}

        public SkillTrack<BaseSkillTrackData> CreateSkillTrack(object param)
        {
            Type type   = typeof(SkillTrack<>);
            type        = type.MakeGenericType(param.GetType());
            object o    = Activator.CreateInstance(type, param);
            SkillTrack<BaseSkillTrackData> skillObj  = o as SkillTrack<BaseSkillTrackData>;
            return skillObj;
        }

        //public SkillClip<BaseSkillClipData> CreateSkillClip(object param)
        //{
        //    Type type   = typeof(SkillClip<>);
        //    type        = type.MakeGenericType(param.GetType());
        //    object o    = Activator.CreateInstance(type, param);
        //    SkillClip<BaseSkillClipData> skillObj = o as SkillClip<BaseSkillClipData>;
        //    return skillObj;
        //}
    }
}
