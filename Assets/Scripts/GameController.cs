using UnityEngine;
using System.Collections;
using System;

public sealed class GameController {

    private static readonly GameController instance = new GameController();
    public static GameController Instance { get { return instance; } }

    private Level currentLevel;

    private GameController() { }

    public void StartGame()
    {
        NextFigure();
    }

    public void NextFigure()
    {
        currentLevel = LevelsCollection.instance.levels[1];
        TaskMenu.Instance.SetFigure(currentLevel.sprite, 10);
        FieldController.Instance.turnAllowed = true;
    }

    public void Comparate(bool[,] usersFigure)
    {
        currentLevel.Compare(usersFigure);
    }
}
