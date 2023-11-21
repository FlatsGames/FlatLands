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

        public static IEnumerator PlayCoroutine(this Animator animator, string animName, string layer = "Base Layer", Action callback = null)
        {
            animator.Play(layer + '.' + animName);
        
            var duration = 0f;
            var animations = animator.runtimeAnimatorController.animationClips;
            foreach (var anim in animations)
            {
                if (anim.name == animName)
                    duration = Mathf.Max(duration, anim.length);
            }

            if (duration>0) 
                yield return new WaitForSeconds(duration);

            callback?.Invoke();
        }
    }
}