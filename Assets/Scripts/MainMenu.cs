using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadTask(int task)
    {
        switch (task)
        {
            case 1:
                SceneManager.LoadSceneAsync("Test1");
                break;
            case 2:
                SceneManager.LoadSceneAsync("Test2");
                break;
            case 3:
                SceneManager.LoadSceneAsync("Test3");
                break;
        }
    }
}
