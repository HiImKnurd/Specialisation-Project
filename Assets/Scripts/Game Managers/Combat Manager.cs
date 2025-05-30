using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] HandManager handManager;
    [SerializeField] Entity player;
    Enemy[] enemies;
    private void OnEnable()
    {
        CardTargeting.playCard += HandleCard;
        CardTargeting.playCardTargeted += HandleCardTargeted;
    }
    private void OnDisable()
    {
        CardTargeting.playCard -= HandleCard;
        CardTargeting.playCardTargeted -= HandleCardTargeted;
    }
    void HandleCard(CardDisplay card)
    {
        PlayCardEffects(card.cardInstance.cardData);
        handManager.DiscardCard(card.gameObject);
    }
    void HandleCardTargeted(CardDisplay card, Enemy target)
    {
        PlayCardEffects(card.cardInstance.cardData, target);
        handManager.DiscardCard(card.gameObject);
    }

    void PlayCardEffects(CardData card, Enemy target = null)
    {
        foreach(CardEffect effect in card.effects)
        {
            bool conditionsMet = true;
            foreach(EffectCondition condition in effect.conditions)
            {
                conditionsMet = ProcessConditions(condition, target);
            }
            if(conditionsMet)
            {
                ProcessEffect(effect, target);   
            }
        }
    }
    bool ProcessConditions(EffectCondition condition, Enemy target = null)
    {
        Entity conditionSubject;
        if (condition.target == EffectCondition.Target.Self)
        {
            conditionSubject = player;
        }
        else // Enemy target
        {
            conditionSubject = target;
        }
        switch(condition.type)
        {
            case EffectCondition.Type.Kill:
                return true;
            default:
                float checkValue;
                if (condition.type == EffectCondition.Type.Health) checkValue = conditionSubject.health;
                else if (condition.type == EffectCondition.Type.Block) checkValue = conditionSubject.block;
                else // Find if target has specified status effect
                {
                    StatusEffect match = target.statusEffects.Find(s => s.effect == condition.status.effect);
                    if (match != null) return true;
                    else return false;
                }
                if (condition.valueRange == EffectCondition.Range.equal)
                {
                    if (checkValue == condition.value) return true;
                    else return false;
                }
                else if (condition.valueRange == EffectCondition.Range.less)
                {
                    if (checkValue < condition.value) return true;
                    else return false;
                }
                else // if (condition.valueRange == EffectCondition.Range.more)
                {
                    if (checkValue > condition.value) return true;
                    else return false;
                }
        }
    }
    void ProcessEffect(CardEffect cardeffect, Enemy enemy = null)
    {
        Entity target = enemy != null ? enemy : player;
        switch (cardeffect.type)
        {
            case CardEffect.EffectType.Damage:
                target.TakeDamage(cardeffect.value);
                break;
            case CardEffect.EffectType.Block:
                target.block += cardeffect.value;
                break;
            case CardEffect.EffectType.Status: 
                StatusEffect match = target.statusEffects.Find(s => s.effect == cardeffect.status.effect);
                if (match != null)
                {
                    match.stacks += cardeffect.value;
                }
                else
                {
                    StatusEffect status = cardeffect.status;
                    target.statusEffects.Add(status);
                    status.stacks = cardeffect.value;
                }
                break;
            case CardEffect.EffectType.Break:
                break;
        }
    }
}
