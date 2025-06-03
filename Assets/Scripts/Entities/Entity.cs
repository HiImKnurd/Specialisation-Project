using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int block;
    public float Atk;
    public float Def;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    // UI Elements
    [SerializeField] Slider healthbar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text blockText;

    protected void Start()
    {
        health = maxHealth;
    }

    protected void Update()
    {
        healthbar.value = (float)health/maxHealth;
        healthText.text = health + "/" + maxHealth;
        blockText.text = block + "";
    }

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
