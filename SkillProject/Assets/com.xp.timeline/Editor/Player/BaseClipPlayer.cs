namespace Timeline.Player
{
    /// <summary>
    /// 基础技能片段播放器
    /// </summary>
    public class BaseClipPlayer : BasePlayer
    {
        private bool isStart = false;
        protected double clipRunningTime;
        protected double clipLeftTime;

        public BaseClipPlayer(BaseTimelineView view) : base(view)
        {
        }

        public override void OnRunningTimeChange(double runningTime)
        {
            ClipData data = (ClipData)View.Data;
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