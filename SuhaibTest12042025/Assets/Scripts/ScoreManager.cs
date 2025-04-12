using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] TextMeshProUGUI lifeText, scoreText, comboText;
    [SerializeField] AudioClip scoreSound, wrongSound, gameOverSound;
    AudioSource audioSource;
    int currentScore = 0;
    int combo = 1;
    int life = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public int GetCurrentScore() { return currentScore; }
    public int GetCombo() { return combo; }
    public int GetLife() { return life; }

    public void ResetScore()
    {
        currentScore = 0;
        combo = 1;
        life = 10;
    }

    public void LoadSaveData(int score, int life, int combo)
    {
        currentScore = score;
        this.life = life;
        this.combo = combo;
        UpdateUI();
    }


    public void RegisterCorrectAnswer()
    {
        currentScore += (1 * combo);
        combo++;
        audioSource.PlayOneShot(scoreSound);
        UpdateUI();
    }

    public void RegisterWrongAnswer()
    {
        life--;
        if (life <= 0)
        {
            ResetScore();
            audioSource.PlayOneShot(gameOverSound);
            AppManager.instance.GameOver();
        }
        else
        {
            combo = 1;
            audioSource.PlayOneShot(wrongSound);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        lifeText.text = life.ToString();
        comboText.text = combo.ToString();
        scoreText.text = currentScore.ToString();
    }
}
