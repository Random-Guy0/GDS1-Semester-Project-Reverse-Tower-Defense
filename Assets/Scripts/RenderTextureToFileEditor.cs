#if(UNITY_EDITOR)
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenderTextureToFileEditor : EditorWindow
{
    private RenderTexture rt;
    private Camera cam;
    
    [MenuItem("Tools/Render Texture to File")]
    public static void ShowWindow()
    {
        GetWindow<RenderTextureToFileEditor>("Render Texture to File");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        rt = (RenderTexture)EditorGUILayout.ObjectField("Render Texture", rt, typeof(RenderTexture), false);
        cam = (Camera)EditorGUILayout.ObjectField("Camera", cam, typeof(Camera), true);
        if (GUILayout.Button("Render"))
        {
            if (rt != null && cam != null)
            {
                string path = AssetDatabase.GetAssetPath(rt) + ".png";
                RenderTextureHandler.SaveRTToFile(rt, cam, path);
            }
        }
        EditorGUILayout.EndVertical();
    }
}

#endif