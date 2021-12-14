using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGenerator levelGenerator = (LevelGenerator)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            levelGenerator.GenerateLevel();
        }
    }
}
