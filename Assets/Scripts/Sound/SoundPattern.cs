using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

[ExecuteInEditMode]
[System.Serializable] //Сериализуемый класс
public class SoundPattern : ScriptableObject {
    
    public AudioClip click;
    public AudioClip bubble;
    public AudioClip perfect;
    public AudioClip win;
    public AudioClip loose;


    private static SoundPattern _instance;                                  //ссылка на себя же

    /// <summary>
    /// Ссылка на текущий (и единственный) экземпляр класса
    /// </summary>
    /// <value>The instance.</value>
    public static SoundPattern instance
    {
        get
        {
            if (_instance == null)                                          //если экземпляр ещё не был создан
            {
                _instance = Resources.Load("SoundEditor") as SoundPattern;  //найдём его в "ресурсах".
                if (_instance == null)                                      //если там его нет
                    _instance = CreateInstance(typeof(SoundPattern)) as SoundPattern; //создадим новый
            }
            return _instance;                                               //вернём ссылку.
        }
    }

    private AudioSource[] audioSources;                                     //массив источников звука
    private const int AUDIO_SOURCES_COUNT = 16;                             //количество источников звука

    /// <summary>
    /// Инициализация.
    /// </summary>
    public void Init()
    {
        GameObject go = (GameObject)Instantiate(new GameObject());          //создаём новый GameObject
        go.name = "Sound Source";                                           //даём ему имя
        audioSources = new AudioSource[AUDIO_SOURCES_COUNT];                //инициализируем массив источников звука
        for (int i = 0; i < AUDIO_SOURCES_COUNT; i ++)                      //идём по их массиву
        {                                                                   //и добавляем к нашему GameObject
            audioSources[i] = go.AddComponent<AudioSource>();               //компоненты AudioSource,
            DontDestroyOnLoad(audioSources[i]);
        }                                                                   //попутно сохраняя ссылки на них в массив
    }

    /// <summary>
    /// Проигрывает нужный звук.
    /// </summary>
    /// <param name="clip">Звук, который нужно проиграть.</param>
    private void Play(AudioClip clip)
    {
        if (clip == null)                                                   //если клип пуст
            return;                                                         //уходим
        for (int i = 0; i < AUDIO_SOURCES_COUNT; i++)                       //идём по массиву источников звука
        {                                                                   
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].clip = clip;
                audioSources[i].Play();
                break;
            }
        }                              
    }


    public static void PlayBubbleSound() { instance.Play(instance.bubble); }
    public static void PlayClickSound() { instance.Play(instance.click); }
    public static void PlayWinSound() { instance.Play(instance.win); }
    public static void PlayPerfectSound() { instance.Play(instance.perfect); }
    public static void PlayLooseSound() { instance.Play(instance.loose); }

}