using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TextMeshProUGUI scoreText;

    public int score = 0;

    void Awake()
    {
        instance = this;
    }

    public void AddPoint()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
}



