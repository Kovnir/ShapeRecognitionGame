using UnityEngine;
using System.Collections;

public sealed class GameController {

    /// защищённый конструктор нужен, чтобы предотвратить создание экземпляра класса Singleton
    private GameController() { }

    private sealed class SingletonCreator
    {
        private static readonly GameController instance = new GameController();
        public static GameController Instance { get { return instance; } }
    }

    public static GameController Instance
    {
        get { return SingletonCreator.Instance; }
    }

    public void StartGame()
    {
        Debug.Log("ad");
    }

    public void NextFigure()
    {
        //LevelsCollection.instance.levels[0].name;
    }

}
