using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Text score = null;
    
    private static GameMenu instance;
    public static GameMenu Instance { get { return instance; } }
    private void Awake() { instance = this; }

    public void SetFigure(Sprite sprite, int time)
    {
        image.sprite = sprite;
    }
    public void SetScore(float score)
    {
        this.score.text = "Score: " + score;
    }

}
