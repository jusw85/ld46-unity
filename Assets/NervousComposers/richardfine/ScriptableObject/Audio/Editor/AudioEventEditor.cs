using Prime31;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioEvent), true)]
public class AudioEventEditor : Editor
{
    // [SerializeField] private AudioSource _previewer;
    [SerializeField] private SoundKit _previewer;

    public void OnEnable()
    {
        // _previewer = EditorUtility
        //     .CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource))
        //     .GetComponent<AudioSource>();
        Debug.Log("Creating");
        _previewer = EditorUtility
            .CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(SoundKit))
            .GetComponent<SoundKit>();
        // _previewer.Awake();
    }

    public void OnDisable()
    {
        Debug.Log("Destroying");
        DestroyImmediate(_previewer.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (GUILayout.Button("Preview"))
        {
            ((AudioEvent) target).Play(_previewer);
        }

        EditorGUI.EndDisabledGroup();
    }
}