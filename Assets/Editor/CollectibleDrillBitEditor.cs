using UnityEditor;

[CustomEditor(typeof(CollectibleDrillBit))]
public class CollectibleDrillBitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CollectibleDrillBit collectibleDrillBit = (CollectibleDrillBit)target;
        if (DrawDefaultInspector())
        {
            collectibleDrillBit.SetType();
        }
    }
}
