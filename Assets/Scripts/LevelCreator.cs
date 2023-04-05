#if(UNITY_EDITOR)
using System;
using UnityEngine;
using UnityEditor;

//A custom tool for creating levels in the editor
public class LevelCreator : EditorWindow
{
    private PathManager pathManager;
    private int levelWidth;
    private int levelDepth;
    private GridTile[] grid;

    private Vector2 scrollPos;
    
    //allows the tool to be opened
    [MenuItem("Tools/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow<LevelCreator>("Level Creator");
    }
    
    //create GUI layout
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        pathManager = (PathManager)EditorGUILayout.ObjectField("Path Manager", pathManager, typeof(PathManager), true);
        if (pathManager != null)
        {
            if (EditorGUI.EndChangeCheck())
            {
                SetInitialValues();
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            levelWidth = EditorGUILayout.IntField("Width", levelWidth);
            EditorGUILayout.Space();
            levelDepth = EditorGUILayout.IntField("Depth", levelDepth);
            if (EditorGUI.EndChangeCheck())
            {
                grid = new GridTile[levelWidth * (levelDepth - 1) + levelWidth];
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < levelDepth; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < levelWidth; j++)
                {
                    int index = levelWidth * i + j;
                    grid[index] = (GridTile)EditorGUILayout.EnumPopup(grid[index], SetEnumPopupColour(grid[index]));
                }
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Level"))
            {
                pathManager.CreateGrid(levelWidth, levelDepth, grid);
            }

            if (GUILayout.Button("Reset Level"))
            {
                pathManager.ResetGrid();
                levelWidth = 0;
                levelDepth = 0;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    //set the colour of text to improve the tool
    private GUIStyle SetEnumPopupColour(GridTile gridTile)
    {
        GUIStyle style = GUI.skin.GetStyle("popup");
        switch (gridTile)
        {
            case GridTile.Ground:
                style.normal.textColor = Color.green;
                break;
            case GridTile.Mountain:
                style.normal.textColor = new Color(150, 75, 0);
                break;
            case GridTile.Path:
            case GridTile.Start:
            case GridTile.End:
                style.normal.textColor = Color.magenta;
                break;
        }
        return style;
    }

    //when selecting a path manager use the values stored in it
    private void SetInitialValues()
    {
        levelWidth = pathManager.GetLevelWidth();
        levelDepth = pathManager.GetLevelDepth();
        grid = pathManager.GetGrid();
    }
}

#endif