using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    public enum SkillPlayState
    {
        Play,
        Stop,
        Pause,
    }

    public class SkillSequence
    {
        private SkillTimer skillTimer;

        private BaseSkillSequenceData data;

        public SkillSequence(BaseSkillSequenceData data)
        {
            this.data  = data;
            skillTimer = new SkillTimer(data.Duration);
        }

        public void Play()
        {
            skillTimer.OnUpdate(OnTimerUpdate);
            skillTimer.Play();
        }

        public void Stop()
        {

        }

        private void OnTimerUpdate(float deltaTime)
        {

        }
    } 
}
