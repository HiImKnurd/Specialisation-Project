using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
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
        Enemy = 0,
        Self = 1
    }
    public enum Type
    {
        Health = 0,
        Block = 1,
        Buff = 2,
        Debuff = 3,
        Kill = 4
    }
    public enum Range
    {
        Equal = 0,
        Less = 1,
        More = 2,
    }
    public Target target;
    public Type type;
    public int value;
    public Range valueRange;
    public string buffName;
    public string debuffName;
}
