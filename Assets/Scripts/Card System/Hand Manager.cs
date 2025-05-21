using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public Transform handTransform; // Root of the hand position
    public float handFanning = 5f; // How much the cards rotate as they spread out in the hand
    public float handSpacing = 20f; // How much the cards space apart in the hand
    public float verticalSpacing = 5f; // How much the cards space out vertically
    public List<GameObject> cardsInHand = new List<GameObject>(); // The cards currently held

    public int handSize = 6;

    private void Start()
    {
        InitialiseCards();
    }
    private void Update()
    {
        UpdateHandVisual();
    }
    void InitialiseCards()
    {
        for(int i = 0; i < handSize; i++)
        {
            AddCardsToHand();
        }
    }

    public void AddCardsToHand()
    {
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        UpdateHandVisual();
    }

    void UpdateHandVisual()
    {
        int cardCount = cardsInHand.Count;
        for(int i = 0; i < cardCount; i++)
        {
            float cardIndex = i - (cardCount - 1) / 2f;
            float normalisedPos = 2f * i/(cardCount - 1) - 1f;
            cardsInHand[i].transform.rotation = Quaternion.Euler(0f, 0f, handFanning * cardIndex);
            cardsInHand[i].transform.localPosition = new Vector3(handSpacing * cardIndex, verticalSpacing * (1 - normalisedPos * normalisedPos), 0f);
        }
    }
}
