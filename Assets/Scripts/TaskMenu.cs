using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TaskMenu : MonoBehaviour {

    [SerializeField]
    private Image image = null;

    private static TaskMenu instance;
    public static TaskMenu Instance { get { return instance; } }
    private void Awake() { instance = this; }

    public void SetFigure(Sprite sprite, int time)
    {
        image.sprite = sprite;
    }
}
