using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlatLands.Architecture
{
    public static class AnimatorUtils
    {
        public static ValueDropdownList<string> GetAvailableAnimationsByCategory(this Animator animator, char categorySymbol = '_', bool includeEmpty = true)
        {
            var animations = GetAvailableAnimations(animator, includeEmpty);
            var categoryAnims = new ValueDropdownList<string>();
            foreach (var animation in animations)
            {
               var str = animation.Split(categorySymbol);
               if(str == null || str.Length < 2)
                   continue;
                   
               var animName = str[1];
               var nameWithCategory = $"{str[0]}/{animName}";
               categoryAnims.Add(nameWithCategory, animation);
            }
            
            return categoryAnims;
        }
        
        public static List<string> GetAvailableAnimations(this Animator animator, bool includeEmpty = true)
        {
            if (animator == null)
                return null;
        
            var list = animator.runtimeAnimatorController.animationClips.Select(element => element.name).ToList();
            if (includeEmpty)
                list.Insert(0, "");
        
            return list;
        }

        public static float GetAnimationDuration(this Animator animator, string animName)
        {
            var duration = 0f;
            var animations = animator.runtimeAnimatorController.animationClips;
            foreach (var anim in animations)
            {
                if (anim.name != animName) 
                    continue;
                
                duration = Mathf.Max(duration, anim.length);
                break;
            }

            return duration;
        }
        
        public static IEnumerator PlayAnimation(this Animator animator, int layerIndex, string animName, Action callback = null)
        {
            var duration = animator.GetAnimationDuration(animName);
            var hashAnim = Animator.StringToHash(animName);
            animator.Play(hashAnim, layerIndex);

            if (duration > 0) 
                yield return new WaitForSeconds(duration);

            callback?.Invoke();
        }

        public static IEnumerator PlayAnimation(this Animator animator, string subStateMachine, string animName, Action callback = null)
        {
            var duration = animator.GetAnimationDuration(animName);
            var hashAnim = Animator.StringToHash(subStateMachine + "." + animName);
            animator.Play(hashAnim);

            if (duration > 0) 
                yield return new WaitForSeconds(duration);

            callback?.Invoke();
        }
        
        public static IEnumerator PlayAnimationWithAction(this Animator animator, string subStateMachine, string animName, float? actionTime = null, Action action = null, Action completeCallback = null)
        {
            var duration = animator.GetAnimationDuration(animName);
            var hashAnim = Animator.StringToHash(subStateMachine + "." + animName);
            animator.Play(hashAnim);

            if(actionTime.HasValue)
            {
                var actionDuration = duration * actionTime.Value;
                yield return new WaitForSeconds(actionDuration);
                action?.Invoke();

                var remainedDuration = duration - actionDuration;
                yield return new WaitForSeconds(remainedDuration);
                completeCallback?.Invoke();
                yield break;
            }

            if (duration > 0)
                yield return new WaitForSeconds(duration);

            completeCallback?.Invoke();
        }
    }
}