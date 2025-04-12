using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsGameManager : MonoBehaviour
{
    public static CardsGameManager Instance;

    CardManager firstSelection;
    CardManager secondSelection;
    bool currentlyOnFirstSelection = true;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterSelection(CardManager card)
    {
        if (currentlyOnFirstSelection)
        {
            currentlyOnFirstSelection = false;
            firstSelection = card;
        }

        else
        {
            currentlyOnFirstSelection = true;
            secondSelection = card;
            if (firstSelection.GetCardNumber() == secondSelection.GetCardNumber())
            {
                ScoreManager.Instance.RegisterCorrectAnswer();
            }

            else
            {
                ScoreManager.Instance.RegisterWrongAnswer();
                firstSelection.Hide();
                secondSelection.Hide();
            }
        }
    }
}
