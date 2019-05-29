using UnityEngine;
using System;
using UnityEditor;

public static class AnimationClipExtensions
{
    static EditorCurveBinding _spriteCurveBinding = new EditorCurveBinding
    {
        type = typeof(SpriteRenderer),
        path = "",
        propertyName = "m_Sprite"
    };

    public static void EditSettings(this AnimationClip clip, Action<AnimationClipSettings> edit)
    {
        var clipSettings = AnimationUtility.GetAnimationClipSettings(clip);

        edit(clipSettings);

        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
    }

    public static ObjectReferenceKeyframe[] GetKeyFrames(this AnimationClip clip)
    {
        return AnimationUtility.GetObjectReferenceCurve(clip, _spriteCurveBinding);
    }

    public static void SetKeyFrames(this AnimationClip clip, ObjectReferenceKeyframe[] keyFrames)
    {
        AnimationUtility.SetObjectReferenceCurve(clip, _spriteCurveBinding, keyFrames);
    }
}
