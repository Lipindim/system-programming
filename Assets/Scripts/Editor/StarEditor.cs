using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarEditor : Editor
{
    private SerializedProperty _center;
    private SerializedProperty _points;
    private SerializedProperty _frequency;

    private GUILayoutOption _buttonWidth = GUILayout.Width(20);
    private GUILayoutOption _buttonHeight = GUILayout.Height(20);

    public bool OpenedFoldable { get; private set; }

    private void OnEnable()
    {
        _center = serializedObject.FindProperty("_center");
        _points = serializedObject.FindProperty("Points");
        _frequency = serializedObject.FindProperty("Frequency");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_center);
        //EditorGUILayout.PropertyField(_points);

        DrawArrayWithoutSize(_points, _points.arraySize);
        EditorGUILayout.IntSlider(_frequency, 1, 20);
        var totalPoints = _frequency.intValue * _points.arraySize;
        
        if (totalPoints < 3)
        {
            EditorGUILayout.HelpBox("At least three points are needed.",
            UnityEditor.MessageType.Warning);
        }
        else
        {
            EditorGUILayout.HelpBox(totalPoints + " points in total.",
            UnityEditor.MessageType.Info);
        }
        if (!serializedObject.ApplyModifiedProperties() &&
            (Event.current.type != EventType.ExecuteCommand ||
            Event.current.commandName != "UndoRedoPerformed"))
        {
            return;
        }
        foreach (var obj in targets)
        {
            if (obj is Star star)
            {
                star.UpdateMesh();
            }
        }
    }

    private void OnSceneGUI()
    {
        if (!(target is Star star))
        {
            return;
        }
        var starTransform = star.transform;
        var angle = -360f / (star.Frequency * star.Points.Count);
        for (var i = 0; i < star.Points.Count; i++)
        {
            var rotation = Quaternion.Euler(0f, 0f, angle * i);
            var oldPoint = starTransform.TransformPoint(rotation *
            star.Points[i].Position);
            var newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity,
            0.02f, star.Points[i].Position, Handles.DotHandleCap);
            if (oldPoint == newPoint)
            {
                continue;
            }
            star.Points[i].Position = Quaternion.Inverse(rotation) *
            starTransform.InverseTransformPoint(newPoint);
            star.UpdateMesh();
        }
    }

    public void DrawArrayWithoutSize(SerializedProperty Property, int ArraySize)
    {
        var bottom = Resources.Load<Texture2D>("bottom");
        var up = Resources.Load<Texture2D>("up");
        var delete = Resources.Load<Texture2D>("delete");
        var add = Resources.Load<Texture2D>("add");

        if (OpenedFoldable = EditorGUILayout.Foldout(OpenedFoldable, "Points"))
        {
            EditorGUI.indentLevel++;

            for (int i = 0; i < ArraySize; ++i)
            {
                SerializedProperty ElementProperty = Property.GetArrayElementAtIndex(i);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(ElementProperty, new GUIContent(string.Concat("Point ", i + 1)));
                DrawSwapButton(up, i, i - 1);
                DrawSwapButton(bottom, i, i + 1);

                if (GUILayout.Button(delete, _buttonWidth, _buttonHeight))
                {
                    DeleteItem(i);
                    return;
                };
                EditorGUILayout.EndHorizontal();
                

            }

            EditorGUI.indentLevel--;

            if (GUILayout.Button(add, _buttonWidth, _buttonHeight))
            {
                AddItem();
                return;
            };
        }
    }

    private void AddItem()
    {
        if (!(target is Star star))
            return;

        star.Points.Add(new ColorPoint());
        star.UpdateMesh();
    }

    private void DeleteItem(int index)
    {
        if (!(target is Star star))
            return;

        star.Points.RemoveAt(index);
        star.UpdateMesh();
    }

    private void DrawSwapButton(Texture2D texture, int startPosition, int finishPosition)
    {
        if (GUILayout.Button(texture, _buttonWidth, _buttonHeight))
        {
            SwapItems(startPosition, finishPosition);
            return;
        };
    }

    private void SwapItems(int first, int second)
    {
        if (!(target is Star star))
            return;

        if (second > star.Points.Count - 1
            || second < 0)
            return;

        var temp = star.Points[first];
        star.Points[first] = star.Points[second];
        star.Points[second] = temp;
        star.UpdateMesh();
    }
}