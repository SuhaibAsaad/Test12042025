using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    int currentScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetScore()
    {
        currentScore = 0;
    }


    public void RegisterCorrectAnswer()
    {

    }

    public void RegisterWrongAnswer()
    {

    }
}
