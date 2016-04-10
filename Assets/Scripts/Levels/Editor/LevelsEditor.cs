using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;

[ExecuteInEditMode]
[CustomEditor(typeof(LevelsCollection))]
public class LevelsEditor : Editor {

    AnimBool m_ShowExtraFields;
    bool IsAddingLevel = false;

    Level additionLevel = new Level();
    Level editingLevel = new Level();
    bool showVisual = false;

    int selectedLevel = -1;
    int selectedGrid = -1;


    uint newGridWidth = 10;
    uint newGridHeight = 10;


    void OnEnable()
    {
        m_ShowExtraFields = new AnimBool(true);
        m_ShowExtraFields.valueChanged.AddListener(Repaint);
    }


    [MenuItem ("Shape Recognition/Create new Level Editor")]
    static void CreateLevelEditor()
    {
        string path = EditorUtility.SaveFilePanel("Create Level Editor",
                                                  "Assets/Scripts/Levels/Resources",
                                                  "LevelEditor.asset", "asset");
        if (path == "")
            return;
        path = FileUtil.GetProjectRelativePath(path);
        LevelsCollection sp = LevelsCollection.instance;
        AssetDatabase.CreateAsset(sp, path);
        AssetDatabase.SaveAssets();
    }

    private LevelsCollection Target
    {
        get { return target as LevelsCollection;} 
    }

    [MenuItem("Shape Recognition/Level Editor")]
    static void OpenSoundEditor()
    {
        AssetDatabase.OpenAsset(LevelsCollection.instance);
    }

    private void DrawAddingPanel()
    {
        EditorGUI.indentLevel++;

        DrawLevel(additionLevel);

        if (GUILayout.Button("Add Level"))
        {
            Target.levels.Add(additionLevel);
            additionLevel = new Level();
            IsAddingLevel = false;
            selectedLevel = Target.levels.Count-1;
            editingLevel = Target.levels[selectedLevel].Clone();
            selectedGrid = -1;
        }
        if (GUILayout.Button("Cancel"))
        {
            IsAddingLevel = false;
        }
        EditorGUI.indentLevel--;


    //    EditorGUILayout.EndFadeGroup();

    }
    private void DrawLevel(Level level)
    {
        level.name = EditorGUILayout.TextField("Name", level.name);
        DrawSprite(ref level.sprite, "Figure Sprite");

        UpdateViewList(level.grids);


        newGridWidth = uint.Parse(EditorGUILayout.TextField("Width", newGridWidth.ToString()));
        newGridHeight = uint.Parse(EditorGUILayout.TextField("Height", newGridHeight.ToString()));
        if (GUILayout.Button("Add New Visual"))
        {
            level.grids.Add(new Level.LevelGrid((int)newGridHeight, (int)newGridWidth));
        }
        if (GUILayout.Button("Remove Visual"))
        {
            level.grids.RemoveAt(selectedGrid);
            selectedGrid = -1;
        }

        if (selectedGrid != -1)
        {
            showVisual = EditorGUILayout.Toggle("Show Visual", showVisual);
            if (showVisual)
            {
                EditorGUILayout.BeginVertical();
                for (int x = 0; x < level.grids[selectedGrid].height; x++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int y = 0; y < level.grids[selectedGrid].width; y++)
                        level.grids[selectedGrid].grid[x/*,y*/][y] = EditorGUILayout.Toggle(level.grids[selectedGrid].grid[x/*, y*/][y]);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }
        Separator();
    }

    private void DrawEditionPanel()
    {
        if (Target.levels == null) Target.levels = new List<Level>();

        if (Target.levels.Count == 0)
        {
            EditorGUILayout.HelpBox("Ещё не было добавлено ни одного уровня.", MessageType.Info);
            if (GUILayout.Button("Добавить первый"))
            {
                IsAddingLevel = true;
            }
        }
        else
        {
            UpdateLevelList(Target.levels);
            if (selectedLevel == -1)
            {
                selectedLevel = 0;
                editingLevel = Target.levels[selectedLevel].Clone();
            }
            DrawLevel(editingLevel);

            if (GUILayout.Button("Save"))
            {
                Target.levels[selectedLevel] = editingLevel;
                editingLevel = new Level();
                selectedLevel = -1;
            }
            if (GUILayout.Button("Add as new"))
            {
                Target.levels.Add(editingLevel);
                editingLevel = new Level();
                selectedLevel = -1;
            }
            if (GUILayout.Button("Remove"))
            {
                Target.levels.RemoveAt(selectedLevel);
                editingLevel = new Level();
                selectedLevel = -1;
            }
            Separator();
            if (GUILayout.Button("Create new Level"))
            {
                IsAddingLevel = true;
                selectedGrid = -1;
                editingLevel = new Level();
            }
        }
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox("В этом окне Вы можете менять игровые уровни. " +
            "Будьте аккуратны!", MessageType.Info);
        if (IsAddingLevel)
            DrawAddingPanel();
        else
            DrawEditionPanel();

        if (GUI.changed)                                //если что-то изменилось
            EditorUtility.SetDirty(target);             //устанавливаем Dirty-флаг для сохранения данных на диск
    }

    private void UpdateLevelList(List<Level> levels)
    {
        string[] options = new string[levels.Count];
        int index = 0;
        levels.ForEach((x) => { options[index] = x.name; index++; });
        int newSelected = EditorGUILayout.Popup("Levels", selectedLevel, options);
        if (selectedLevel != newSelected)
        {
            selectedLevel = newSelected;
            editingLevel = Target.levels[selectedLevel].Clone();
            selectedGrid = -1;
        }
    }
    private void UpdateViewList(List<Level.LevelGrid> grids)
    {
        string[] options = new string[grids.Count];
        int index = 0;
        grids.ForEach((x) => { options[index] = x.height.ToString()+"x"+ x.width.ToString(); index++; });
        int newSelected = EditorGUILayout.Popup("Views", selectedGrid, options);
        if (selectedGrid != newSelected)
        {
            selectedGrid = newSelected;
        }
    }
    private void DrawSprite(ref Sprite sprite, string name)
    {
        sprite = EditorGUILayout.ObjectField(name, sprite, typeof(Sprite), false) as Sprite;
    }
    private void Separator()
    {
        GUILayout.Label("_________________________________________________________________________________________________________________________________________________________________________");
    }
}
