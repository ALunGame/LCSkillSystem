using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    public enum SkillTimerState
    {
        Play,
        Stop,
        Pause,
    }

    public class SkillTimer
    {
        //获得当前时间委托
        public static Func<float> GetCurrTime;

        private float timer;
        private float endTime;
        private float duration;
        private SkillTimerState CurrSate = SkillTimerState.Stop;

        private Action onPlay;
        private Action onPause;
        private Action onResume;
        private Action onStop;
        private Action<float> onUpdate;

        private float currRunTime;

        //当前运行到那个时间
        public float CurrRunTime { get => currRunTime;}

        public SkillTimer(float duration)
        {
            this.duration = duration;
        }

        public void Play()
        {
            Stop();
            endTime  = GetCurrTime() + duration;
            timer    = GetCurrTime();
            CurrSate = SkillTimerState.Play;
            currRunTime = 0;
            onPlay?.Invoke();
        }

        public void Pause()
        {
            CurrSate = SkillTimerState.Pause;
            onPause?.Invoke();
        }

        public void Resume()
        {
            if (CurrSate != SkillTimerState.Pause)
                return;
            float leftTime = endTime - timer;
            if (leftTime <= 0)
                return;

            endTime  = GetCurrTime() + leftTime;
            timer    = GetCurrTime();
            CurrSate = SkillTimerState.Play;
            onResume?.Invoke();
        }

        public void Stop()
        {
            CurrSate = SkillTimerState.Stop;
            onStop?.Invoke();
        }

        public void Update(float deltaTime)
        {
            if (CurrSate != SkillTimerState.Play)
                return;
            if (timer >= endTime)
            {
                Stop();
                return;
            }
            timer += deltaTime;
            currRunTime += deltaTime;
            onUpdate?.Invoke(deltaTime);
        }

        #region Reg

        public SkillTimer OnPlay(Action onPlay)
        {
            this.onPlay = onPlay;
            return this;
        }

        public SkillTimer OnPause(Action onPause)
        {
            this.onPause = onPause;
            return this;
        }

        public SkillTimer OnResume(Action onResume)
        {
            this.onResume = onResume;
            return this;
        }

        public SkillTimer OnStop(Action onStop)
        {
            this.onStop = onStop;
            return this;
        }

        public SkillTimer OnUpdate(Action<float> onUpdate)
        {
            this.onUpdate = onUpdate;
            return this;
        }

        #endregion
    }
}
