namespace LCSkill
{
    public abstract class SkillObject
    {
        public abstract void Init();

        public abstract void Play();

        public abstract void Pause();

        public abstract void Resume();

        public abstract void Stop();

        public abstract void Update(float deltaTime);
    }
}
