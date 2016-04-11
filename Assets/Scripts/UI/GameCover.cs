using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameCover : MonoBehaviour {

    private static GameCover instance;
    public static GameCover Instance { get { return instance; } }

    [SerializeField]
    private Text endText;

    void Awake()
    {
        instance = this;
    }


    public void GameStart()
    {
        GameController.Instance.StartGame();
    }

    public void GameOver()
    {
        Invoke("GameOverShow", 1);
    }
    private void GameOverShow()
    {
        endText.text = "Game Over =(";
        GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void FiguresOver()
    {
        Invoke("FiguresOverShow", 1);
    }

    private void FiguresOverShow()
    {
        endText.text = "No more figures =)";
        GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }
}
