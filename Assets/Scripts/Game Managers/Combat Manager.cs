using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] HandManager handManager;
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
        handManager.DiscardCard(card.gameObject);
    }
    void HandleCardTargeted(CardDisplay card, Enemy target = null)
    {
        handManager.DiscardCard(card.gameObject);
    }

}
