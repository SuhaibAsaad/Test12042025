using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    public int cardNumber;

    public void LoadData(SerializableCardData cardData)
    {
       cardNumber = cardData.cardNumber; 
    }

}

[Serializable]
public class SerializableCardData
{
    public int cardNumber;

    public SerializableCardData(CardData cardData)
    {
        cardNumber = cardData.cardNumber;
    }
}
