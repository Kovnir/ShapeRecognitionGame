using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;

public class ThreadManager : MonoBehaviour {

    object sync = new object();
    List<Action> actions = new List<Action>();

    private static ThreadManager instance;
    public static ThreadManager Instance { get { return instance; } }
    private void Awake() { instance = this; }

    void Update()
    {
        lock (sync)
        { //обеспечиваем потокобезопасность чтения листа
            while (actions.Count != 0)
            { //и исполняем все действия
                actions[0].Invoke();
                actions.RemoveAt(0);
            }
        }
    }
    //Данный метод нельзя вызывать из основного потока, ни в коем случае.
    public void Execute(Action action)
    {
        lock (sync)
        { //обеспечиваем потокобезопасность записи в лист
            actions.Add(action);
        }
        try
        {
            Thread.Sleep(1);//усыпляем вызвавший поток
        }
        catch (ThreadInterruptedException)
        {
        }
        finally { }
    }
}
