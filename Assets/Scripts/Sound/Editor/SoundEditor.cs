using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(SoundPattern))]
public class SoundEditor : Editor {

    //    [MenuItem ("Shape Recognition/Create new Sound Editor")]
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
        DrawClip(ref Target.coverOnClip, "Cover On");
        DrawClip(ref Target.coverOffClip, "Cover Off");
        DrawClip(ref Target.buttonClickClip, "Button Click");
        DrawClip(ref Target.catchClip, "Catch");
        DrawClip(ref Target.dropClip, "Drop");
        DrawClip(ref Target.failDropClip, "Fail Drop");
        DrawClip(ref Target.matchClip, "Match");
        DrawClip(ref Target.looseClip, "Loose");
        DrawClip(ref Target.newCardClip, "New Card");
        DrawClip(ref Target.startCountdownClip, "Start Countdown");
        DrawClip(ref Target.tickClip, "Tick");

        if (GUI.changed)                                //если что-то изменилось
            EditorUtility.SetDirty(target);             //устанавливаем Dirty-флаг для сохранения данных на диск
    }


    private void DrawClip(ref AudioClip clip, string name)
    {
        clip = EditorGUILayout.ObjectField(name, clip, typeof(AudioClip), false) as AudioClip;
    }
}
