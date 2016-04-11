using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private Text score = null;

    public void Awake()
    {
        if (GameStats.lastScore != -1)
        {
//            if (GameStats.bestScore > GameStats.lastScore)
//                score.text = "Score: " + GameStats.lastScore + "\nBest: " + GameStats.lastScore;
//            if (GameStats.bestScore == GameStats.lastScore)
                score.text = "Best score = " + GameStats.bestScore;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
    public void PlayButton()
    {
        GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
