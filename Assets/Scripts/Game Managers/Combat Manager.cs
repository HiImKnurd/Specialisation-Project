using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] HandManager handManager;
    [SerializeField] Entity player;
    [SerializeField] Enemy[] enemies;
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
        PlayEffects(card.cardInstance.cardData);
        handManager.DiscardCard(card.gameObject);
    }
    void HandleCardTargeted(CardDisplay card, Enemy target)
    {
        PlayEffects(card.cardInstance.cardData, target);
        handManager.DiscardCard(card.gameObject);
    }

    public void CycleTurns()
    {
        //handManager.DiscardAll();

        foreach(Enemy enemy in enemies)
        {
            PlayEnemyActions(enemy);
        }

        handManager.DrawHand();

        foreach(Enemy enemy in enemies)
        {
            enemy.ChooseActions();
        }
    }

    void PlayEnemyActions(Enemy enemy)
    {
        foreach(EnemyAction action in enemy.turnActions)
        {
            foreach(Effect effect in action.effects)
            {
                if (effect.target == Effect.Target.Enemy)
                {
                    ProcessEffect(effect);
                }
                else ProcessEffect(effect, enemy);
            }
        }
        enemy.turnActions.Clear();
    }

    void PlayEffects(CardData card, Enemy target = null)
    {
        foreach(Effect effect in card.effects)
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
    void ProcessEffect(Effect Effect, Enemy enemy = null)
    {
        Entity target = enemy != null ? enemy : player;
        switch (Effect.type)
        {
            case Effect.EffectType.Damage:
                target.TakeDamage(Effect.value);
                break;
            case Effect.EffectType.Block:
                target.block += Effect.value;
                break;
            case Effect.EffectType.Status: 
                StatusEffect match = target.statusEffects.Find(s => s.effect == Effect.status.effect);
                if (match != null)
                {
                    match.stacks += Effect.value;
                }
                else
                {
                    StatusEffect status = Effect.status;
                    target.statusEffects.Add(status);
                    status.stacks = Effect.value;
                }
                break;
            case Effect.EffectType.Break:
                break;
        }
    }
}
