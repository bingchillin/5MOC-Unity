using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int score = 0;
    public int health = 100;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;

    public void UpdateScore(int amount)
    {
        score += amount;
    }

    void Update()
    {
        scoreText.text = "SCORE: " + score;
        healthText.text = "HEALTH: " + health; 
    }

    public void UpdateHealth(int amount)
    {
        health += amount;
    }
}
