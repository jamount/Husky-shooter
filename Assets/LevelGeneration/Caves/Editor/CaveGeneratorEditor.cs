using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CaveGenerator))]
public class CaveGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        CaveGenerator mapGen = (CaveGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}