using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(SoundPattern))]
public class SoundEditor : Editor {

    [MenuItem ("Shape Recognition/Create new Sound Editor")]
    static void CreateSoundEditor()
    {
        string path = EditorUtility.SaveFilePanel("Create Sound Editor",
                                                  "Assets/Scripts/Sounds/Resources",
                                                  "SoundEditor.asset", "asset");
        if (path == "")
            return;
        path = FileUtil.GetProjectRelativePath(path);
        SoundPattern sp = SoundPattern.instance;
        AssetDatabase.CreateAsset(sp, path);
        AssetDatabase.SaveAssets();
    }

    private SoundPattern Target
    {
        get { return target as SoundPattern;} 
    }

    [MenuItem("Shape Recognition/Sound Editor")]
    static void OpenSoundEditor()
    {
        AssetDatabase.OpenAsset(SoundPattern.instance);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("В этом окне Вы можете менять все звуки, " +
            "присутвующие в игре.", MessageType.Info);

        DrawClip(ref Target.click, "Click");
        DrawClip(ref Target.bubble, "Bubble");
        DrawClip(ref Target.loose, "Loose");
        DrawClip(ref Target.win, "Win");
        DrawClip(ref Target.perfect, "Perfect");

        if (GUI.changed)                                //если что-то изменилось
            EditorUtility.SetDirty(target);             //устанавливаем Dirty-флаг для сохранения данных на диск
    }


    private void DrawClip(ref AudioClip clip, string name)
    {
        clip = EditorGUILayout.ObjectField(name, clip, typeof(AudioClip), false) as AudioClip;
    }
}
