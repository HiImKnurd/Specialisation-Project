using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInstance
{
    CardTemplate cardTemp;
    public CardData cardData { get; private set; }

    public CardInstance(CardTemplate cardTemp)
    {
        this.cardTemp = cardTemp;
        cardData = cardTemp.cardData;
    }
}
