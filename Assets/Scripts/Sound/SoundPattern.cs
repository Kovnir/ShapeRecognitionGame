using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

[ExecuteInEditMode]
[System.Serializable] //Сериализуемый класс
public class SoundPattern : ScriptableObject {
    
    public AudioClip coverOnClip;                                           //Опускание ширмы экранного перехода
    public AudioClip coverOffClip;                                          //Поднятие ширмы экранного перехода
    public AudioClip buttonClickClip;                                       //Клик на кнопке
    public AudioClip catchClip;                                             //Взятие карты
    public AudioClip dropClip;                                              //Бросание карты на стопку                              
    public AudioClip failDropClip;                                          //Бросание карты мимо стопки (или на заполненную стопку)
    public AudioClip matchClip;                                             //Собирание слова (исчезновение начисление очков)
    public AudioClip looseClip;                                             //Проигрыш
    public AudioClip newCardClip;                                           //Появление новой карты
    public AudioClip startCountdownClip;                                    //Начальный отсчёт (три секундных отсчёта и надпись “старт”)
    public AudioClip tickClip;                                              //звук тиканья таймера

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


    public static void PlayCovenOnSound() { instance.Play(instance.coverOnClip); }
    public static void PlayCovenOffSound() { instance.Play(instance.coverOffClip); }
    public static void PlayButtonClickSound() { instance.Play(instance.buttonClickClip); }
    public static void PlayCatchSound() { instance.Play(instance.catchClip); }
    public static void PlayDropSound() { instance.Play(instance.dropClip); }
    public static void PlayFailDropSound() { instance.Play(instance.failDropClip); }
    public static void PlayMatchSound() { instance.Play(instance.matchClip); }
    public static void PlayLooseSound() { instance.Play(instance.looseClip); }
    public static void PlayNewCardSound() { instance.Play(instance.newCardClip); }
    public static void PlayStartCountdownSound() { instance.Play(instance.startCountdownClip); }
    public static void PlayTickSound() { instance.Play(instance.tickClip); }
}