using UnityEngine;

public static class SpriteExtensions
{
    public static string GetSpritesheetName(this Sprite[] sprites)
    {
        var spriteName = sprites[0].name;
        spriteName = spriteName.Substring(0, spriteName.IndexOf("_", System.StringComparison.Ordinal));

        return spriteName;
    }
}
