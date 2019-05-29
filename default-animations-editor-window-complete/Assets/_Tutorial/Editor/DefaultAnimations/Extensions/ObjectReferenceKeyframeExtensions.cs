using UnityEngine;
using UnityEditor;

public static class ObjectReferenceKeyframeExtensions
{
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
