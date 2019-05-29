using UnityEngine;
using System;
using UnityEditor;
using System.Linq;

public static class Texture2DExtensions
{
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
