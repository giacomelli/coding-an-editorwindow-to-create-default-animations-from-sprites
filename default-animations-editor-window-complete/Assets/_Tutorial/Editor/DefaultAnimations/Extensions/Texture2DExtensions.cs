using UnityEngine;
using UnityEditor;
using System.Linq;

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
        return assets.OfType<Sprite>().ToArray();
    }
}
