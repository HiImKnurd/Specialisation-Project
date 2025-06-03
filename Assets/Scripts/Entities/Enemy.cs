using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Entity
{
    public GameObject highlight;
    public string enemyName;
    [SerializeField] TMP_Text nameText;

    new void Start()
    {
        nameText.text = enemyName;
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }
}
