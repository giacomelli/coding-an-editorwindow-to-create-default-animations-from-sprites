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
        if (sprites.Length == 0)
            Debug.LogError("sprites should have at least one element");

        return sprites[0].GetSpritesheetName();
    }

    /// <summary>
    /// Gets the name of the spritesheet.
    /// </summary>
    /// <returns>The spritesheet name.</returns>
    /// <param name="sprite">Sprite.</param>
    public static string GetSpritesheetName(this Sprite sprite)
    {
       return sprite.name.Substring(0, sprite.name.IndexOf("_", System.StringComparison.Ordinal));
    }
}
