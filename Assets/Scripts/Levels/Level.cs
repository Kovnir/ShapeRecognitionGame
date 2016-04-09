using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable] //Сериализуемый класс
public class Level {

    [Serializable]
    public class LevelGrid
    {
        public int MAX_HEIGHT = 10;
        public int MAX_WIDTH = 15;
        public int height;
        public int width;
        public bool[,] grid;
        
        public LevelGrid(int height, int width)
        {
            this.height = height;
            this.width = width;

            grid = new bool[height,width];
        }

        public LevelGrid Clone()
        {
            return new LevelGrid(height, width) { grid = grid};
        }
    }

    public Sprite sprite;
    public string name;
    public List<LevelGrid> grids = new List<LevelGrid>();
        

    public Level Clone()
    {
        Level level = new Level() { sprite = sprite, name = name};
        level.grids = new List<LevelGrid>();
        grids.ForEach((x) => level.grids.Add(x.Clone()));
        return level;
    }
}
