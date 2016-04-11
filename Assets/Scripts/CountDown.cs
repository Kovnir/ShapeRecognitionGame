using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {

    IEnumerator coroutine;
    Text text;

    private static CountDown instance;
    public static CountDown Instance { get { return instance; } }
    private bool isPaused = false;


    void Awake()
    {
        instance = this;
        text = GetComponent<Text>();
    }

	public void StartCountDown (int time) {
        Stop();
        isPaused = false;
        coroutine = CountDownCoroutine(time);
        StartCoroutine(coroutine);
	}
	
    public void Pause()
    {
        isPaused = true;
    }

    public void Stop()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

    IEnumerator CountDownCoroutine(int time)
    {
        while (time > -1)
        {
            text.text = time.ToString();
            yield return new WaitForSeconds(1);
            if (!isPaused) time--;
        }
        GameController.Instance.GameOver();
    }
}
