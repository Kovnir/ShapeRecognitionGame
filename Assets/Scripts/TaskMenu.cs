using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskMenu : MonoBehaviour {

    [SerializeField]
    private Image image = null;
    [SerializeField]
    private Text score = null;
    
    private static TaskMenu instance;
    public static TaskMenu Instance { get { return instance; } }
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
