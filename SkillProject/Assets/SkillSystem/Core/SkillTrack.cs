using System.Collections.Generic;

namespace LCSkill
{
    public class SkillTrack<T> : SkillObject where T : BaseSkillTrackData
    {
        public T TrackData;
        //public List<SkillClip<BaseSkillClipData>> ClipList = new List<SkillClip<BaseSkillClipData>>();

        public SkillTrack(T data)
        {
            //TrackData = data;
            //for (int i = 0; i < TrackData.ClipDatas.Count; i++)
            //{
            //    var clip = TrackData.ClipDatas[i];
            //    ClipList.Add(SkillLocate.SkillServer.CreateSkillClip(clip));
            //}
        }

        public override void Init()
        {
        }

        public override void Pause()
        {
        }

        public override void Play()
        {
        }

        public override void Resume()
        {
        }

        public override void Stop()
        {
        }

        public override void Update(float deltaTime)
        {


        }
    } 
}
