using UnityEngine;
using System.Collections.Generic;

public static class AnimatorOverrideControllerExtensions
{
    public static void ApplyOverrides(this AnimatorOverrideController animatorOverride, AnimationClip[] fromClips, AnimationClip[] toClips)
    {
        if (fromClips.Length != toClips.Length)
            Debug.LogError($"fromClips and toClips should have the same number of elements");

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < toClips.Length; i++)
        {
            overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(fromClips[i], toClips[i]));
        }

        animatorOverride.ApplyOverrides(overrides);
    }
}
