
#if EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
 
[CustomEditor(typeof(InfiniteScrollBackgroundElement))]
public class InfiniteScrollBackgroundGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InfiniteScrollBackgroundElement instance = (InfiniteScrollBackgroundElement)target;
        if (GUILayout.Button("Generate"))
        {
            instance.GenerateSprites();  
        }

    }
}

#endif