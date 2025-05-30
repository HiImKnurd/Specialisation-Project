using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int block;
    public float Atk;
    public float Def;

    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public void TakeDamage(int damage)
    {
        int healthlost = damage;
        if(block > 0)
        {
            block -= damage;
            if (block < 0)
            {
                healthlost = -block;
                block = 0;
            }
            else return;
        }
        health -= healthlost;
        Debug.Log(gameObject.name + " lost " + healthlost + " health.");
    }

}
