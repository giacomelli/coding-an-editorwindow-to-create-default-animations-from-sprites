using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Settings for DefaultAnimationsEditorWindow.
/// http://diegogiacomelli.com.br/coding-an-editorwindow-to-create-default-animations-from-sprites/
/// </summary>
public class DefaultAnimationsSettings : ScriptableObject
{
    public RuntimeAnimatorController AnimatorController;
    public AnimationMapping[] AnimationsMapping;
    public string AnimationsFolder;
    public Texture2D[] Spritesheets;

    static DefaultAnimationsSettings _instance;

    public static DefaultAnimationsSettings Instance => _instance ?? (_instance = LoadAsset());

    private static DefaultAnimationsSettings LoadAsset()
    {
        var path = GetAssetPath();
        var asset = AssetDatabase.LoadAssetAtPath<DefaultAnimationsSettings>(path);

        if (asset == null)
        {
            asset = CreateInstance<DefaultAnimationsSettings>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        }

        return asset;
    } 

    private static string GetAssetPath([CallerFilePath] string callerFilePath = null)
    {
        var folder = Path.GetDirectoryName(callerFilePath);
        folder = folder.Substring(folder.LastIndexOf("/Assets/", StringComparison.Ordinal) + 1);

        return Path.Combine(folder, "DefaultAnimationsSettings.asset");
    }

    [Serializable]
    public class AnimationMapping
    {
        int[] _spriteIndexes = null;
        public string Name;
        public AnimationClip ClipToOverride;
       
        public int[] SpriteIndexes 
        {
            get
            {
                if (_spriteIndexes == null || _spriteIndexes.Length == 0)
                {
                    var keyFrames = ClipToOverride.GetKeyFrames();
                    var spritesheet = ((Sprite)(keyFrames[0].value)).texture;
                    var sprites = spritesheet.GetSprites().Select((s, index) => new { sprite = s, index }).ToArray();
                    _spriteIndexes = new int[keyFrames.Length];

                    for (int i = 0; i < keyFrames.Length; i++)
                    {
                        var sprite = (Sprite)keyFrames[i].value;
                        _spriteIndexes[i] = sprites.First(s => s.sprite.name == sprite.name).index;
                    }
                }

                return _spriteIndexes;
            }
        }
    }
}
