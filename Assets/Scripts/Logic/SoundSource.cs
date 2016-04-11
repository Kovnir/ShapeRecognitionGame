using UnityEngine;
using System.Collections;

public class SoundSource : MonoBehaviour {
    
    private void Start()
    {
        SoundPattern.instance.Init();                               //инициализирем звуковой паттерн
        MusicPattern.instance.SetAuidioSource(ThreadManager.Instance.gameObject.GetComponent<AudioSource>());

        MusicPattern.instance.PlayMainMenuMusic();
    }

// Update is called once per frame
void Update () {
	
	}
}
