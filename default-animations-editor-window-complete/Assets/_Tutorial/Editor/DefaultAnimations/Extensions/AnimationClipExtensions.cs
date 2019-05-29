using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// Animation clip extensions.
/// </summary>
public static class AnimationClipExtensions
{
    static EditorCurveBinding _spriteCurveBinding = new EditorCurveBinding
    {
        type = typeof(SpriteRenderer),
        path = "",
        propertyName = "m_Sprite"
    };

    /// <summary>
    /// Edits the settings.
    /// </summary>
    /// <param name="clip">Clip.</param>
    /// <param name="editCallback">The edit callback to change the settings.</param>
    public static void EditSettings(this AnimationClip clip, Action<AnimationClipSettings> editCallback)
    {
        var clipSettings = AnimationUtility.GetAnimationClipSettings(clip);

        editCallback(clipSettings);

        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
    }

    /// <summary>
    /// Gets the sprite key frames of the animation clip.
    /// </summary>
    /// <returns>The key frames.</returns>
    /// <param name="clip">Clip.</param>
    public static ObjectReferenceKeyframe[] GetKeyFrames(this AnimationClip clip)
    {
        return AnimationUtility.GetObjectReferenceCurve(clip, _spriteCurveBinding);
    }

    /// <summary>
    /// Sets the sprite key frames of the animation clip.
    /// </summary>
    /// <param name="clip">Clip.</param>
    /// <param name="keyFrames">Key frames.</param>
    public static void SetKeyFrames(this AnimationClip clip, ObjectReferenceKeyframe[] keyFrames)
    {
        AnimationUtility.SetObjectReferenceCurve(clip, _spriteCurveBinding, keyFrames);
    }
}
