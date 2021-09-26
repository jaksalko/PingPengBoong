using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        base.OnInspectorGUI();

        if(GUILayout.Button("Edit"))
        {
            mapGenerator.BlockPositionEditor();
        }
    }
}
