using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

[System.Serializable] //Сериализуемый класс
public class MusicPattern : ScriptableObject {

    public AudioClip mainMusic;                         //главное меню
                                                            //продлеваем ли переходящие в 
    private bool extendTheRepetitiveTracks = true; //следующую сцену треки
    private AudioSource musicAudioSource;                   //источник звука

    private bool _isMusic = true;                                  //включена ли музыка
    public bool isMusic                                     //публичный геттер/сеттер
    {
        set
        {
            _isMusic = value;                               //устанавливаем новое значение
            if (!value)                                     //если это false
                musicAudioSource.Stop();                    //останавлииваем воспроизведение
            else                                            //иначе
            {                                               //установим музыку настроект, так как
             //   musicAudioSource.clip = settingsMusic;      //только в настройках можем включить звук
                musicAudioSource.Play();                    //играем её
            }
        }
        get
        {
            return _isMusic;                                //возвращаем значение
        }
    }

    private static MusicPattern _instance;                  //ссылка на себя же
    /// <summary>
    /// Ссылка на текущий (и единственный) экpемпляр класса
    /// </summary>
    /// <value>The instance.</value>
    public static MusicPattern instance
    {
        get
        {
            if (_instance == null)                                          //если экземпляр ещё не был создан
            {
                _instance = Resources.Load("MusicEditor") as MusicPattern;  //найдём его в "ресурсах".
                if (_instance == null)                                      //если там его нет
                    _instance = CreateInstance(typeof(MusicPattern))        //создадим новый
                        as MusicPattern; 
            }
            return _instance;                                               //вернём ссылку.
        }
    }

    /// <summary>
    /// Уставовка источника звука.
    /// </summary>
    /// <param name="audioSource"></param>
    public void SetAuidioSource(AudioSource audioSource)
    {
        musicAudioSource = audioSource;                                     //присвоим новый источник
        musicAudioSource.priority = 0;                                      //установим высший приоритет
        musicAudioSource.loop = true;                                       //играем циклично
    }

    /// <summary>
    /// Останавливаем музыку.
    /// </summary>
    public void Stop()
    {
        musicAudioSource.Stop();
    }
	
    /// <summary>
    /// Проигрываем заданный аудиоклип.
    /// </summary>
    /// <param name="clip">аудиоклип, который надо проиграть</param>
    private void Play(AudioClip clip)
    {
        if (!isMusic)                                   //если музыка отключена
            return;                                     //уходим
        if (extendTheRepetitiveTracks)                  //если продлеваем повторяющиеся треки
            if (musicAudioSource.clip == clip)          //и сейчас проигрывается нужный клип
                return;                                 //уходим
        musicAudioSource.Stop();                        //останавливаем предыдущую музыку
        musicAudioSource.clip = clip;                   //ставим новую
        musicAudioSource.Play();                        //играем
    }

    public void PlayMainMenuMusic()     { Play(mainMusic); }    //играть музыку 
}
