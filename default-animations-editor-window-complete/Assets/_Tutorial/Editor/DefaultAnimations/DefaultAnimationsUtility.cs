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
        var clip = GetAsset<AnimationClip>($"{GetTextureName(sprites)}-{map.Name}");
        clip.frameRate = map.ClipToOverride.frameRate;
        clip.wrapMode = map.ClipToOverride.wrapMode;

        // In more complex animation clip, maybe will be needed to copy more settings.
        var clipToOverrideSettings = AnimationUtility.GetAnimationClipSettings(map.ClipToOverride);
        clip.EditSettings(s => s.loopTime = clipToOverrideSettings.loopTime);

        var keyFramesToOverride = map.ClipToOverride.GetKeyFrames();
        var keyFrames = new ObjectReferenceKeyframe[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe
            {
                // Copy time from clip to override.
                time = keyFramesToOverride[i].time,
                value = sprites[i]
            };
        }

        clip.SetKeyFrames(keyFrames);
        SaveAsset(clip);

        return clip;
    }

    private static AnimatorOverrideController CreateAnimatorOverride(Sprite[] sprites, AnimationClip[] overrideClips)
    {
        var settings = DefaultAnimationsSettings.Instance;
        var animatorOverride = GetAsset<AnimatorOverrideController>(GetTextureName(sprites));
        animatorOverride.runtimeAnimatorController = settings.AnimatorController;

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        for (int i = 0; i < overrideClips.Length; i++)
        {
            overrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(settings.AnimationsMapping[i].ClipToOverride, overrideClips[i]));
        }

        animatorOverride.ApplyOverrides(overrides);

        SaveAsset(animatorOverride);

        return animatorOverride;
    }

    private static (bool canCreate, int spritesNeed) Validate(DefaultAnimationsSettings settings, Sprite[] sprites)
    {
        var spritesNeed = settings.AnimationsMapping.SelectMany(x => x.SpriteIndexes).Distinct().Count();

        return (sprites.Length >= spritesNeed, spritesNeed);
    }

    private static TAsset GetAsset<TAsset>(string name)
        where TAsset : Object, new()
    {
        var path = GetAssetPath(name, typeof(TAsset));
        return AssetDatabase.LoadAssetAtPath<TAsset>(path) ?? new TAsset { name = name };
    }

    private static void SaveAsset(Object asset)
    {
        var path = GetAssetPath(asset.name, asset.GetType());
        var existingAsset = AssetDatabase.LoadAssetAtPath(path, asset.GetType());
       
        if (existingAsset == null)
        {
            AssetDatabase.CreateAsset(asset, path);
            Debug.Log($"'{path}' created.");
        }
        else
            Debug.Log($"'{path}' updated.");
    }

    private static string GetAssetPath(string name, System.Type assetType)
    {
        return $"{DefaultAnimationsSettings.Instance.AnimationsFolder}/{name}{GetAssetExtension(assetType)}";
    }

    private static string GetAssetExtension(System.Type assetType)
    {
        if (assetType == typeof(AnimationClip))
            return ".anim";

        if (assetType == typeof(AnimatorOverrideController))
            return ".overrideController";

        Debug.LogWarning($"Returning .asset has extension for {assetType}.");

        return ".asset";
    }

    private static string GetTextureName(Sprite[] sprites)
    {
        var spriteName = sprites[0].name;
        spriteName = spriteName.Substring(0, spriteName.IndexOf("_", System.StringComparison.Ordinal));

        return spriteName;
    }
}
