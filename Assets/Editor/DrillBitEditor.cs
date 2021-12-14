using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(DrillBit))]
public class DrillBitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrillBit drillBit = (DrillBit)target;
        DrawDefaultInspector();
    }
}