using Timeline.Data;
using Timeline.View;

namespace Timeline.Player
{
    /// <summary>
    /// 基础技能片段播放器
    /// </summary>
    public class BaseClipPlayer
    {
        private BaseClipView clipView;
        private bool isStart = false;
        protected double clipRunningTime;
        protected double clipLeftTime;

        protected BaseClipView ClipView
        {
            get { return clipView; }
        }

        public BaseClipPlayer(BaseClipView clipView)
        {
            this.clipView = clipView;
        }

        public void OnRunningTimeChange(double runningTime)
        {
            ClipData data = clipView.Data;
            clipRunningTime = runningTime - data.StartTime;
            clipLeftTime = data.EndTime - runningTime;

            //结束判断
            if (runningTime < data.StartTime || runningTime > data.EndTime)
            {
                EndPlay();
                return;
            }

            //开始判断
            if (runningTime >= data.StartTime)
            {
                StartPlay();
            }

            //正在判断
            if (runningTime >= data.StartTime && runningTime <= data.EndTime)
            {
                Playing();
            }
        }

        #region Private

        private void StartPlay()
        {
            if (isStart)
            {
                OnStart();
                isStart = true;
            }
        }

        private void Playing()
        {
            if (isStart)
            {
                OnPlaying();
            }
        }

        private void EndPlay()
        {
            isStart = false;
            OnEnd();
        }

        #endregion Private

        #region Virtual

        /// <summary>
        /// 开始播放
        /// </summary>
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// 正在播放
        /// </summary>
        public virtual void OnPlaying()
        {
        }

        /// <summary>
        /// 结束
        /// </summary>
        public virtual void OnEnd()
        {
        }

        #endregion Virtual
    }
}