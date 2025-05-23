using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardInstance card;
    CardData cardData;

    [SerializeField] Image cardImage;
    [SerializeField] TMP_Text cardCost;
    [SerializeField] TMP_Text cardName;
    [SerializeField] TMP_Text cardDescription;
    [SerializeField] TMP_Text cardType;
    public string descriptionText = "";

    private void Start()
    {
        
    }

    public void UpdateCardDisplay()
    {
        cardData = card.cardData;
        if (cardData == null) return;

        cardImage.sprite = cardData.cardImage;
        cardCost.text = cardData.cardCost + "";
        cardName.text = cardData.cardName;
        cardType.text = cardData.type.ToString();

        UpdateCardDescription();
        cardDescription.text = descriptionText;
    }

    private void Update()
    {
        
    }

    private void UpdateCardDescription()
    {
        CardEffect effect;
        for (int i = 0; i < cardData.effects.Length; i++)
        {
            Debug.Log("Add effect out of " + cardData.effects.Length);
            effect = cardData.effects[i];
            if (effect.type == CardEffect.EffectType.Break)
            {
                descriptionText += "Break. ";
                continue;
            }
            if (effect.target == CardEffect.Target.Self)
            {
                descriptionText += $"Gain {effect.value} {(effect.effectName != "" ? effect.effectName : effect.type.ToString().ToLower())}";
            }
            else
            {
                descriptionText += $"Deal {effect.value} {(effect.effectName != "" ? effect.effectName : effect.ToString().ToLower())}";
            }

            if (effect.conditions.Length == 0)
            {
                descriptionText += ". ";
                continue;
            }
            string conditiontext = "";
            for (int x = 0; x < effect.conditions.Length; x++)
            {
                if (conditiontext != "") conditiontext += " and";
                EffectCondition condition = effect.conditions[x];
                conditiontext += $" if {(condition.target == 0 ? "your" : "target's")}" +
                    $"{(condition.type == EffectCondition.Type.Kill ? " killed." : $" {(condition.type == EffectCondition.Type.Status ? condition.statusName : condition.type.ToString())} is {(condition.valueRange == 0 ? "equal to" : condition.valueRange.ToString() + " than")} {condition.value}")}";
            }
            descriptionText += conditiontext;
            descriptionText += ". ";
        }
        cardDescription.text = descriptionText;
    }
}
