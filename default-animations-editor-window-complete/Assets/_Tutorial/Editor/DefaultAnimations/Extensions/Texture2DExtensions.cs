using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Texture2 DE xtensions.
/// </summary>
public static class Texture2DExtensions
{
    /// <summary>
    /// Gets the sprites from the spritesheet.
    /// </summary>
    /// <returns>The sprites.</returns>
    /// <param name="spritesheet">Spritesheet.</param>
    public static Sprite[] GetSprites(this Texture2D spritesheet)
    {
        var path = AssetDatabase.GetAssetPath(spritesheet);

        var importer = (TextureImporter) AssetImporter.GetAtPath(path);

        if (importer.textureType != TextureImporterType.Sprite || importer.spriteImportMode != SpriteImportMode.Multiple)
        {
            Debug.LogError($"The Texture2D should be a sprite and importe mode should be 'Multiple'.");
        }

        var assets = AssetDatabase.LoadAllAssetsAtPath(path);

        // To keep sprites in the natural order.
        return assets.OfType<Sprite>().OrderBy(s => int.Parse(s.name.Substring(s.name.IndexOf("_", StringComparison.Ordinal) + 1))).ToArray();
    }
}
