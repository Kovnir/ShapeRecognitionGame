using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable] //Сериализуемый класс
public class Level {

    [Serializable]
    public class LevelGrid
    {
        [Serializable]
        public struct GridLine
        {
            public bool[] line;
            public bool this[int i]
            {
                get
                {
                    return line[i];
                }
                set
                {
                    line[i] = value;
                }
            }
            public GridLine(int length) { line = new bool[length]; }
        }
        public GridLine[] grid;

        public int MAX_HEIGHT = 10;
        public int MAX_WIDTH = 15;
        public int height;
        public int width;

        public LevelGrid(int height, int width)
        {
            this.height = height;
            this.width = width;

            grid = new GridLine[height];
            for (int i = 0; i < height; i++) grid[i] = new GridLine(width);
        }

        public LevelGrid Clone()
        {
            return new LevelGrid(height, width) { grid = grid };
        }

        private bool[,] GetArray()
        {
            bool[,] result = new bool[height, width];
            for (int x = 0; x < height ; x++)
                for (int y = 0; y < width; y++)
                    result[x, y] = grid[x][y];
            return result;
        }
 
        public float Comparate(bool[,] usersFigure, out int xOfsetOut, out int yOfsetOut)
        {
            float best = 0;
            int bestXOffset = 0;
            int bestYOffset = 0;
            bool templateIsSmaller = true;

            bool[,] smallHeightFigure = GetArray();            //меньшая по ширине фигура
            bool[,] bigHeightFigure = usersFigure;       //большая по ширине фигура
            //если не угадали - поменяем местами
            if (smallHeightFigure.GetLength(0) > bigHeightFigure.GetLength(0))
            {
                bool[,] buffer = smallHeightFigure;
                smallHeightFigure = bigHeightFigure;
                bigHeightFigure = buffer;
                templateIsSmaller = !templateIsSmaller;
            }

            bigHeightFigure = Extend(bigHeightFigure, bigHeightFigure.GetLength(0) + 2, bigHeightFigure.GetLength(1), 1, 0);

            for (int x = 0; x < bigHeightFigure.GetLength(0) - smallHeightFigure.GetLength(0) + 1; x++)
            {
                int xOffset = 0;
                bool[,] smallWidthFigure = Extend(smallHeightFigure, bigHeightFigure.GetLength(0), smallHeightFigure.GetLength(1), x, 0);
                if (templateIsSmaller)
                {
                    xOffset = x-1;
                }
                //увеличим до нужной ширины
                bool[,] bigWidthFigure = bigHeightFigure;       //большая по высоте фигура
                                                                //если не угадали - поменяем местами
                if (smallWidthFigure.GetLength(1) > bigWidthFigure.GetLength(1))
                {
                    bool[,] buffer = smallWidthFigure;
                    smallWidthFigure = bigWidthFigure;
                    bigWidthFigure = buffer;
                    templateIsSmaller = !templateIsSmaller;
                }

                bigWidthFigure = Extend(bigWidthFigure, bigWidthFigure.GetLength(0), bigWidthFigure.GetLength(1)+2, 0, 1);

                for (int y = 0; y < bigWidthFigure.GetLength(1) - smallWidthFigure.GetLength(1) + 1; y++)
                {
                    int yOffset = 0;
                    bool[,] smallWidthModifiedFigure = Extend(smallWidthFigure, smallWidthFigure.GetLength(0), bigWidthFigure.GetLength(1), 0, y);   //увеличим до нужной ширины
                    if (templateIsSmaller)
                    {
                        yOffset = y-1;
                    }
                    float newScore = SimpleComparate(smallWidthModifiedFigure, bigWidthFigure);
                    if (newScore > best)
                    {
                        best = newScore;
                        bestXOffset = xOffset;
                        bestYOffset = yOffset;
                        if (best == 1)
                        {
                            xOfsetOut = bestXOffset;
                            yOfsetOut = bestYOffset;

                            return best;
                        }
                    }
                }
            }
            xOfsetOut = bestXOffset;
            yOfsetOut = bestYOffset;
            return best;
        }

        public static bool[,] Extend(bool[,] matrix, int newHeight, int newWidth, int newX, int newY)
        {
            bool[,] newMatrix = new bool[newHeight, newWidth];
            for (int x = 0; x < matrix.GetLength(0); x++)
                for (int y = 0; y < matrix.GetLength(1); y++)
                    newMatrix[x + newX, y + newY] = matrix[x, y];
            return newMatrix;
        }
        public bool[,] Extend(int newHeight, int newWidth, int newX, int newY)
        {
            return Extend(GetArray(), newHeight, newWidth, newX, newY);
        }
        private float SimpleComparate(bool[,] matrix1, bool[,] matrix2)
        {
            float maximum = matrix1.GetLength(0) * matrix1.GetLength(1);
            float count = 0;
            for (int x = 0; x < matrix1.GetLength(0); x++)
                for (int y = 0; y < matrix1.GetLength(1); y++)
                    if (matrix1[x, y] == matrix2[x, y]) count++;

             return count/ maximum;
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

    public void SortGrids()
    {
        grids = grids.OrderBy((x) => x.height*x.width).ToList();
    }

    public void Compare(bool[,] usersFigure)
    {
        Thread thread = null;

        Action threadAction = () =>
        {
            float result = float.MinValue;
            int xOffset = 0;
            int yOffset = 0;
            Level.LevelGrid resultGrid = null;
            foreach (LevelGrid grid in grids)
            {
                float newResult = grid.Comparate(usersFigure, out xOffset, out yOffset);
                if (newResult > result)
                {
                    result = newResult;
                    resultGrid = grid;
                    if (result == 1)
                        break;
                }
            }
            ThreadManager.Instance.Execute(() =>
            {
                FieldController.Instance.ComporateFinish(resultGrid.Clone(), xOffset, yOffset, result);
                thread.Interrupt(); //данное действие очень важно в конце всей последовательности
            });
        };

        thread = new Thread(new ThreadStart(threadAction));
        thread.Start();
    }

    private static void CalculateCallback(float result)
    {
        TaskMenu.Instance.SetScore(result);
    }
}
