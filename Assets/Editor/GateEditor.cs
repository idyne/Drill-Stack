using UnityEditor;

[CustomEditor(typeof(DrillSlot))]
public class GateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrillSlot gate = (DrillSlot)target;
        if (DrawDefaultInspector())
        {
            gate.SetPower();
        }
    }
}
