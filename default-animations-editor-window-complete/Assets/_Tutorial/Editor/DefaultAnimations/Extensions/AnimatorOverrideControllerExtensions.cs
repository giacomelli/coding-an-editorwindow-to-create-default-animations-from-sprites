using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Animator override controller extensions.
/// </summary>
public static class AnimatorOverrideControllerExtensions
{
    /// <summary>
    /// Apply the animation clip overrides betweem fromClips and toClips.
    /// </summary>
    /// <param name="animatorOverride">Animator override.</param>
    /// <param name="fromClips">From clips.</param>
    /// <param name="toClips">To clips.</param>
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
