using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Entity
{
    public GameObject highlight;
    public string enemyName;
    [SerializeField] TMP_Text nameText;

    public List<EnemyAction> actionList = new List<EnemyAction>();
    public List<EnemyAction> turnActions = new List<EnemyAction>();

    public int maxEnergy = 1;
    public int turnEnergy;

    new void Start()
    {
        nameText.text = enemyName;
        turnEnergy = maxEnergy;
        base.Start();
        ChooseActions();
    }
    new void Update()
    {
        base.Update();
    }

    public void ChooseActions()
    {
        turnEnergy = maxEnergy;
        while (turnEnergy > 0) {
            EnemyAction chosen = WeightedRandomSelection();
            turnActions.Add(chosen);
            turnEnergy -= chosen.energyCost;
        }
    }

    EnemyAction WeightedRandomSelection()
    {
        float totalWeight = 0f, culmulative = 0f;
        foreach (EnemyAction action in actionList)
        {
            if(action.energyCost <= turnEnergy) totalWeight += action.selectionWeight;
        }
        float randomValue = UnityEngine.Random.value * totalWeight;
        foreach (EnemyAction action in actionList)
        {
            if (action.energyCost > turnEnergy) continue;
            culmulative += action.selectionWeight;
            if (randomValue <= culmulative) return action;
        }
        return null;
    }
}
[Serializable]
public class EnemyAction
{
    public string name;
    public int energyCost = 1;
    public float selectionWeight = 1f; // Likelihood to be chosen
    public Effect[] effects;
    public enum ActionTypes
    {
        SingleAtk,
        MultiAtk,
        Block,
        Buff,
        Debuff
    }
    public ActionTypes actionType;
}