using UnityEngine;
using System.Collections;

public class GameCover : MonoBehaviour {

    public void GameStart()
    {
        GameController.Instance.StartGame();
    }
}
