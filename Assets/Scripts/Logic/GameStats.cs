using UnityEngine;
using System.Collections;

public static class GameStats
{

    private static int _lastScore = -1;
    public static int lastScore
    {
        get
        {
            return _lastScore;
        }
    }
    private static int _bestScore = -1;
    public static int bestScore
    {
        get
        {
            return _bestScore;
        }
    }

    public static bool SetScore(uint score)
    {
        _lastScore = (int)score;
        if (_lastScore > _bestScore)
        {
            _bestScore = _lastScore;
            return true;
        }
        return false;
    }
}
