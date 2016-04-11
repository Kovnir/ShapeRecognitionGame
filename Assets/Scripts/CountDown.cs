using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountDown : MonoBehaviour {

    IEnumerator coroutine;
    Text text;

    private static CountDown instance;
    public static CountDown Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        text = GetComponent<Text>();
    }

	public void StartCountDown (int time) {
        coroutine = CountDownCoroutine(time);
        StartCoroutine(coroutine);
	}
	
    IEnumerator CountDownCoroutine(int time)
    {
        while (time > 0)
        {
            text.text = time.ToString();
            yield return new WaitForSeconds(0);
            time--;
        }
    }
}
