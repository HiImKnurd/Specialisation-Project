using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardTemplate : ScriptableObject
{
    public CardData cardData;
}

[System.Serializable]
public class CardData
{
    public enum cardType
    {
        Attack = 0,
        Defence = 1,
        Utility = 2,
        Debuff = 3
    }
    public cardType type;
    public string cardName;
    public int cardCost;
    public string cardDescription;
    public bool targeted;
    public Sprite cardImage;

    public CardEffect[] effects;
}

[System.Serializable]
public class CardEffect
{
    public enum Target
    {
        Enemy = 0,
        Self = 1
    }
    public enum EffectType
    {
        Damage = 0,
        Block = 1,
        Buff = 2,
        Debuff = 3,
        Break = 4
    }
    public Target target;
    public EffectType type;
    public int value;
    public bool mainEffect;

    public string effectName;
    public string buffName;
    public string debuffName;

    public EffectCondition[] conditions;
}
[System.Serializable]
public class EffectCondition
{
    public enum Target
    {
        Self = 0,
        Enemy = 1
    }
    public enum Type
    {
        Health = 0,
        Block = 1,
        Status = 2,
        Kill = 3
    }
    public enum Range
    {
        equal = 0,
        less = 1,
        more = 2,
    }
    public Target target;
    public Type type;
    public int value;
    public Range valueRange;
    public string statusName;
}
