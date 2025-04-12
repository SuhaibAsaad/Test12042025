using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager instance;
    [SerializeField] CardsLayoutManager cardsLayoutManager;
    [SerializeField] TMP_InputField rowsInput, columnsInput;
    [SerializeField] TextMeshProUGUI layoutErrorMessage;
    [SerializeField] GameObject gameOverText;
    string savePath;


    private void Awake()
    {
        savePath = Application.dataPath + "/saveData.json";
        instance = this;
    }
    public void GenerateNewLayout()
    {
        CardsGameManager.Instance.ResetSelection();
        layoutErrorMessage.gameObject.SetActive(true); 
        layoutErrorMessage.text = cardsLayoutManager.GenerateCards(int.Parse(rowsInput.text), int.Parse(columnsInput.text));
    }

    public void Save()
    {
        SaveData saveData = new SaveData();

        saveData.rows = cardsLayoutManager.currentRows;
        saveData.columns = cardsLayoutManager.currentColumns;

        foreach (CardManager cardManager in cardsLayoutManager.cards)
        {
            saveData.cards.Add(cardManager.GetSaveData());
        }

        saveData.score = ScoreManager.Instance.GetCurrentScore();
        saveData.life = ScoreManager.Instance.GetLife();
        saveData.combo = ScoreManager.Instance.GetCombo();

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            return;
        }
        string json = File.ReadAllText(savePath);
        SaveData loadData = JsonUtility.FromJson<SaveData>(json);

        cardsLayoutManager.GenerateCardsFromSaveData(loadData.rows, loadData.columns, loadData.cards);
        ScoreManager.Instance.LoadSaveData(loadData.score, loadData.life, loadData.combo);
    }

    public void GameOver()
    {
        GenerateNewLayout();
        StartCoroutine(ShowGameOverText());
    }

    IEnumerator ShowGameOverText()
    {
        gameOverText.SetActive(true);
        yield return new WaitForSeconds(5f);
        gameOverText.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Save();
        }


        if (Input.GetKeyDown(KeyCode.F5))
        {
            Load();
        }
    }
}

[Serializable]
public class SaveData
{
    public int rows;
    public int columns;
    public List<CardSaveData> cards = new List<CardSaveData>();
    public int score;
    public int life;
    public int combo;
}
