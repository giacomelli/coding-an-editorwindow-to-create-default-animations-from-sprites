using UnityEngine;
using UnityEditor;

/// <summary>
/// Object reference keyframe extensions.
/// </summary>
public static class ObjectReferenceKeyframeExtensions
{
    /// <summary>
    /// Creates a nem array ObjectReferenceKeyframe base on current one, but using the sprites specified.
    /// </summary>
    /// <returns>The new ObjectReferenceKeyframe array.</returns>
    /// <param name="sourceKeyFrames">Source key frames.</param>
    /// <param name="sprites">Sprites.</param>
    public static ObjectReferenceKeyframe[] CreateWithSprites(this ObjectReferenceKeyframe[] sourceKeyFrames, Sprite[] sprites)
    {
        var keyFrames = new ObjectReferenceKeyframe[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe
            {
                // Copy time from clip to override.
                time = sourceKeyFrames[i].time,
                value = sprites[i]
            };
        }
        
        return keyFrames;
    }
}
