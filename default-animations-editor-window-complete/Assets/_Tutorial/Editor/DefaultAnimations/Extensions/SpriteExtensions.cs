using UnityEngine;

/// <summary>
/// Sprite extensions.
/// </summary>
public static class SpriteExtensions
{
    /// <summary>
    /// Gets the name of the spritesheet.
    /// </summary>
    /// <returns>The spritesheet name.</returns>
    /// <param name="sprites">Sprites.</param>
    public static string GetSpritesheetName(this Sprite[] sprites)
    {
        var spriteName = sprites[0].name;
        spriteName = spriteName.Substring(0, spriteName.IndexOf("_", System.StringComparison.Ordinal));

        return spriteName;
    }
}
