using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGen))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUI.changed) {
            Debug.Log("Changed");
            (target as TerrainGen).GenTerrain(1);        //(target as TerrainGen).GenTerrain(1);
        }

    }
}
