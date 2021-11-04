using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XPToolchains.Help
{
    /// <summary>
    /// 编辑器下播放各种效果
    /// </summary>
    public static class EditorPlayHelper
    {
        /// <summary>
        /// 播放动画
        /// </summary>
        public static void PlayAnim(GameObject animGo, string animName, float animTime)
        {
            if (animGo == null)
                return;

            Animation animation = animGo.GetComponent<Animation>();
            if (animation != null)
            {
                PlayAnimation(animation, animName, animTime);
                return;
            }

            Animator animator = animGo.GetComponent<Animator>();
            if (animator != null)
            {
                PlayAnimator(animator, animName, animTime);
                return;
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        public static void PlayAnimation(Animation anim, string animName, float animTime)
        {
            if (anim == null)
                return;
            AnimationState state = anim[animName];
            if (state == null)
                return;

            anim.Play(state.name);
            state.time = animTime;
            state.speed = 0f;
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        public static void PlayAnimator(Animator anim, string animName, float animTime)
        {
            if (anim == null)
                return;
            anim.Play(animName);
            anim.playbackTime = animTime;
            anim.Update(0);
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public static void PlayParticle(ParticleSystem psys, float time)
        {
            if (psys == null)
                return;
            psys.useAutoRandomSeed = false;
            psys.Simulate(time);
        }
    }
}