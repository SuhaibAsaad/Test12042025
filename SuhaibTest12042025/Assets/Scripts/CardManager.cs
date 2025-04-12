using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] RectTransform cover, face;
    [SerializeField] TextMeshProUGUI cardNumberText;
    float cardFlipSpeed = 1f;
    CardData cardData;
    bool canBeSelected = true;
    bool unlocked = false;
    
    public int GetCardNumber()
    {
        return cardData.cardNumber;
    }
    public void AssignCard(CardData data)
    {
        cardNumberText.text = data.cardNumber.ToString();
        cardData = data;
    }

    public void AssignCardFromSaveData(CardSaveData data)
    {
        cardNumberText.text = data.cardData.cardNumber.ToString();
        cardData = new CardData(data.cardData);
        unlocked = data.unlocked;
        if(unlocked)
        {
            Show(false);
        }
    }

    public void Select()
    {
        if(!canBeSelected) { return; }
        canBeSelected = false;
        Show();
    }

    public void RegisterAsUnlocked()
    {
        unlocked = true;
    }

    public CardSaveData GetSaveData()
    {
        return new CardSaveData(cardData,unlocked);
    }

    public void Show(bool selectedByuser = true)
    {
        RotateAroundYAxis(cover, 90f, () =>
        {
            cover.gameObject.SetActive(false);
            face.gameObject.SetActive(true);
            RotateAroundYAxis(face, 180, () =>
            {
                if (selectedByuser)
                {
                    CardsGameManager.Instance.RegisterSelection(this);
                }
            });
        });
    }

    public void Hide()
    {
        RotateAroundYAxis(face, 90f, () =>
        {
            face.gameObject.SetActive(false);
            cover.gameObject.SetActive(true);
            RotateAroundYAxis(cover, 0f, () =>
            {
                canBeSelected = true;
            });
        });
    }


    void RotateAroundYAxis(RectTransform targetTransform, float targetRotationInY, Action callback = null)
    {
        StartCoroutine(RotateRectTransform(targetTransform, new Vector3(0f, targetRotationInY, 0f), callback));
    }

    IEnumerator RotateRectTransform(RectTransform targetTransform, Vector3 targetRotation, Action callback = null)
    {
        Vector3 startingRotation = targetTransform.rotation.eulerAngles;
        float elapsed = 0f;

        while (elapsed < cardFlipSpeed / 2)
        {
            targetTransform.rotation = Quaternion.Euler(Vector3.Lerp(startingRotation, targetRotation, elapsed / (cardFlipSpeed / 2)));
            elapsed += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Show();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Hide();
        }
    }
}

[Serializable]
public class CardSaveData
{
    public SerializableCardData cardData;
    public bool unlocked;

    public CardSaveData(CardData cardData, bool unlocked)
    {
        this.cardData = new SerializableCardData(cardData);
        this.unlocked = unlocked;
    }
}
