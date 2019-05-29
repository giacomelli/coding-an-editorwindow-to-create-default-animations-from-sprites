using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Utility class used by DefaultAnimationsEditorWindow to perform operations.
/// http://diegogiacomelli.com.br/coding-an-editorwindow-to-create-default-animations-from-sprites/
/// </summary>
public static class DefaultAnimationsUtility
{
    /// <summary>
    /// Create the default animations (Animation Clips and Animator Override Controller) based on Animations Mapping.
    /// </summary>
    /// <param name="spritesheets">The spritesheets to create the default animations.</param>
    public static void Create(params Texture2D[] spritesheets)
    {
        var assetsCreated = new List<Object>();

        for (int i = 0; i < spritesheets.Length; i++)
        {
            var spritesheet = spritesheets[i];
            EditorUtility.DisplayProgressBar("Default Animations", $"Creating default animations for {spritesheet.name}", i / (float)(spritesheets.Length));

            var settings = DefaultAnimationsSettings.Instance;
            var sprites = spritesheet.GetSprites();
            var (canCreate, spritesNeed) = Validate(settings, sprites);

            if (!canCreate)
            {
                Debug.LogWarning ($"Need {spritesNeed} sprites, but there are {sprites.Length} selected. Check the sprites selected or change animations settings on Window / Default Animations");
                continue;
            }

            var clips = CreateAnimationClips(sprites, settings);
            var animatorOverride = CreateAnimatorOverride(sprites, clips);
           
            assetsCreated.AddRange(clips);
            assetsCreated.Add(animatorOverride);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();

        // Selects the created clips on project view.
        Selection.objects = assetsCreated.ToArray();
    }

    private static AnimationClip[] CreateAnimationClips(Sprite[] sprites, DefaultAnimationsSettings settings)
    {
        var mappings = settings.AnimationsMapping;
        var clips = new AnimationClip[mappings.Length];

        for (int i = 0; i < mappings.Length; i++)
        {
            var map = mappings[i];
            clips[i] = CreateAnimationClip(map, settings, sprites.Where((sprite, index) => map.SpriteIndexes.Contains(index)).ToArray());
        }

        return clips;
    }

    private static AnimationClip CreateAnimationClip(DefaultAnimationsSettings.AnimationMapping map, DefaultAnimationsSettings settings, params Sprite[] sprites)
    {
        var clip = AssetHelper.GetAsset<AnimationClip>($"{sprites.GetSpritesheetName()}-{map.Name}");
        clip.frameRate = map.ClipToOverride.frameRate;
        clip.wrapMode = map.ClipToOverride.wrapMode;

        // In more complex animation clip, maybe will be needed to copy more settings.
        var clipToOverrideSettings = AnimationUtility.GetAnimationClipSettings(map.ClipToOverride);
        clip.EditSettings(s => s.loopTime = clipToOverrideSettings.loopTime);

        // Create the keyframes based on source animation, but with the new sprites.
        var keyFramesToOverride = map.ClipToOverride.GetKeyFrames();
        var keyFrames = keyFramesToOverride.CreateWithSprites(sprites);
        clip.SetKeyFrames(keyFrames);

        clip.SaveAsset();

        return clip;
    }

    private static AnimatorOverrideController CreateAnimatorOverride(Sprite[] sprites, AnimationClip[] overrideClips)
    {
        var settings = DefaultAnimationsSettings.Instance;
        var animatorOverride = AssetHelper.GetAsset<AnimatorOverrideController>(sprites.GetSpritesheetName());
        animatorOverride.runtimeAnimatorController = settings.AnimatorController;

        animatorOverride.ApplyOverrides(
            settings.AnimationsMapping.Select(m => m.ClipToOverride).ToArray(), 
            overrideClips);

        animatorOverride.SaveAsset();

        return animatorOverride;
    }

    private static (bool canCreate, int spritesNeed) Validate(DefaultAnimationsSettings settings, Sprite[] sprites)
    {
        var spritesNeed = settings.AnimationsMapping.SelectMany(x => x.SpriteIndexes).Distinct().Count();

        return (sprites.Length >= spritesNeed, spritesNeed);
    }
}
