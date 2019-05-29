using UnityEditor;
using UnityEngine;

/// <summary>
/// Asset helper.
/// </summary>
public static class AssetHelper
{
    /// <summary>
    /// Gets the asset with the specified name from the DefaultAnimationsSettings.Instance.AnimationsFolder.
    /// </summary>
    /// <returns>The asset.</returns>
    /// <param name="name">Name.</param>
    /// <typeparam name="TAsset">The 1st type parameter.</typeparam>
    public static TAsset GetAsset<TAsset>(string name)
    where TAsset : Object, new()
    {
        var path = GetAssetPath(name, typeof(TAsset));
        return AssetDatabase.LoadAssetAtPath<TAsset>(path) ?? new TAsset { name = name };
    }

    /// <summary>
    /// Saves the asset to the DefaultAnimationsSettings.Instance.AnimationsFolder.
    /// </summary>
    /// <param name="asset">Asset.</param>
    public static void SaveAsset(this Object asset)
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
}
