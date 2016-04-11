using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

[ExecuteInEditMode]
[Serializable] //Сериализуемый класс
public class LevelsCollection : ScriptableObject {

    public List<Level> levels;

    private static LevelsCollection _instance;                                  //ссылка на себя же

    /// <summary>
    /// Ссылка на текущий (и единственный) экземпляр класса
    /// </summary>
    /// <value>The instance.</value>
    public static LevelsCollection instance
    {
        get
        {
            if (_instance == null)                                          //если экземпляр ещё не был создан
            {
                _instance = Resources.Load("LevelEditor") as LevelsCollection;  //найдём его в "ресурсах".
                if (_instance == null)                                      //если там его нет
                    _instance = CreateInstance(typeof(LevelsCollection)) as LevelsCollection; //создадим новый
            }
            return _instance;                                               //вернём ссылку.
        }
    }

}