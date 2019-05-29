using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Default animations editor window.
/// http://diegogiacomelli.com.br/coding-an-editorwindow-to-create-default-animations-from-sprites/
/// </summary>
public class DefaultAnimationsEditorWindow : EditorWindow
{
    DefaultAnimationsSettings _settings;
    SerializedObject _settingsSO;
    SerializedProperty _animatorController;
    SerializedProperty _animationsMapping;
    SerializedProperty _animationsFolder;
    SerializedProperty _spritesheets;
    bool _showSettings;
    bool _showOperations;

    [MenuItem("Window/Default Animations")]
    public static void ShowWindow()
    {
        GetWindow<DefaultAnimationsEditorWindow>("Default Animations");
    }

    private void OnEnable()
    {
        // Read the settings from the ScriptableObject and initialize the
        // SeleriazedObject and SerializedProperty.
        _settings = DefaultAnimationsSettings.Instance;
        _settingsSO = new SerializedObject(_settings);
        _animatorController = _settingsSO.FindProperty("AnimatorController");
        _animationsMapping = _settingsSO.FindProperty("AnimationsMapping");
        _animationsFolder = _settingsSO.FindProperty("AnimationsFolder");
        _spritesheets = _settingsSO.FindProperty("Spritesheets");
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        DrawSettingsSection();
        DrawOperationsSection();

        // If user made any change on editor window field, then
        // apply the changes directly on scriptable object.
        if (EditorGUI.EndChangeCheck())
            _settingsSO.ApplyModifiedProperties();
    }

    private void DrawSettingsSection()
    {
        _showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_showSettings, "Settings");

        if (_showSettings)
        {
            EditorGUILayout.PropertyField(_animatorController);
            EditorGUILayout.PropertyField(_animationsMapping, true);
            EditorGUILayout.PropertyField(_animationsFolder);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawOperationsSection()
    {
        if (_animatorController.objectReferenceValue == null
        || _animationsMapping.arraySize == 0
        || string.IsNullOrEmpty(_animationsFolder.stringValue))
            return;

        _showOperations = EditorGUILayout.BeginFoldoutHeaderGroup(_showOperations, "Operations");

        if (_showOperations)
        {
            EditorGUILayout.PropertyField(_spritesheets, true);

            if (_settings.Spritesheets != null
            && _settings.Spritesheets.Length > 0
            &&  GUILayout.Button("Create animations"))
                DefaultAnimationsUtility.Create(_settings.Spritesheets);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
