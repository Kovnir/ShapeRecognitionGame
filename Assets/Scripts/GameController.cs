using UnityEngine;
using System.Collections;
using System;
using Random = System.Random;
using System.Collections.Generic;

public sealed class GameController {

    private static readonly GameController instance = new GameController();
    public static GameController Instance { get { return instance; } }

    private int currentLevel;
    private List<Level> levels;
    private List<int> times;
    
    private GameController() { }

    public void StartGame()
    {
        levels = LevelsCollection.instance.levels;
        levels.ForEach((x) =>x.SortGrids());
        MixList(levels);
        currentLevel = -1;
        InitializeTimeList();
        NextFigure();
    }

    //список времени на прохождение уровня в секундах
    private void InitializeTimeList()
    {
        times = new List<int>();
        times.Add(10);
        times.Add(9);
        times.Add(8);
        times.Add(7);
        times.Add(6);
        times.Add(5);
        times.Add(4);
        times.Add(4);
        times.Add(4);
        times.Add(4);
    }

    internal void GameOver()
    {
        GameCover.Instance.GameOver();
    }

    public void NextFigure()
    {
        currentLevel++;
        if (levels.Count  == currentLevel)
        {
            GameCover.Instance.FiguresOver();
            CountDown.Instance.Stop();
            return;
        }
        Level level = levels[currentLevel];
        TaskMenu.Instance.SetFigure(level.sprite, 10);
        CountDown.Instance.StartCountDown(times[currentLevel]);
        FieldController.Instance.turnAllowed = true;
    }

    public void Comparate(bool[,] usersFigure)
    {
        levels[currentLevel].Compare(usersFigure);
    }

    public static void MixList<t>(IList<t> list)
    {
        Random r = new Random();
        //Создаем сортируемый список и набиваем в него значения из 
        //целевого списка. Тип SortedList устроен так, что при добавлении 
        //нового элемента, он (элемент) помещается не в конец списка 
        //элементов, а между ними, обеспечивая моментальную автоматическую 
        //сортировку списка
        SortedList<int, t> mixedList = new SortedList<int, t>();
        //заполняем список
        foreach (t item in list)
            mixedList.Add(r.Next(), item);

        list.Clear();
        for (int i = 0; i < mixedList.Count; i++)
        {
            list.Add(mixedList.Values[i]);
        }
    }

}
