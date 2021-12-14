using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(Wall))]
public class WallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Wall wall = (Wall)target;
        if (DrawDefaultInspector())
        {
            wall.Initiate();
        }
    }
}