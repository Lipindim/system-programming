using UnityEditor;
using UnityEngine;

public class PlanetsWindow : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _planetPrefabProperty;
    private SerializedProperty _centerPointProperty;
    private SerializedProperty _planetsProperty;

    [MenuItem("Window/Planets Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PlanetsWindow));
    }

    private void Awake()
    {
        var planets = Resources.Load<PlanetsInfo>("Planets");
        _serializedObject = new SerializedObject(planets);
        _planetPrefabProperty = _serializedObject.FindProperty("PlanetPrefab");
        _centerPointProperty = _serializedObject.FindProperty("CenterPosition");
        _planetsProperty = _serializedObject.FindProperty("Planets");
    }


    private void OnGUI()
    {
        EditorGUILayout.PropertyField(_planetPrefabProperty);
        EditorGUILayout.PropertyField(_centerPointProperty);
        EditorGUILayout.PropertyField(_planetsProperty);

        _serializedObject.ApplyModifiedProperties();
    }
}