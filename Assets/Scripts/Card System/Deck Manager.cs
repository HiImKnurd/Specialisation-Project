using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<CardInstance> cards = new List<CardInstance>();
    [SerializeField] CardTemplate defaultAttack;
    [SerializeField] CardTemplate defaultDefend;

    [SerializeField] int maxDeckSize = 20;

    private void Start()
    {
        int defaultCount = maxDeckSize / 4;
        for(int i = 0; i < defaultCount; i++)
        {
            cards.Add(new CardInstance(defaultAttack));
            cards.Add(new CardInstance(defaultDefend));
        }
    }
}
