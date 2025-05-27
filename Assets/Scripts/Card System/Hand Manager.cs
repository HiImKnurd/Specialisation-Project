using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public Transform handTransform; // Root of the hand position
    public float handFanning = 5f; // How much the cards rotate as they spread out in the hand
    public float handSpacing = 20f; // How much the cards space apart in the hand
    public float verticalSpacing = 5f; // How much the cards space out vertically
    public List<GameObject> cardsInHand = new List<GameObject>(); // The cards currently held
    public List<GameObject> cardsInDraw = new List<GameObject>();
    public List<GameObject> cardsInDiscard = new List<GameObject>();

    [SerializeField] DeckManager deck;

    public int handSize = 6;

    private void Start()
    {
        InitialiseCards();
    }
    private void Update()
    {
        //UpdateHandVisual();
    }
    void InitialiseCards()
    {
        foreach(CardInstance card in deck.cards)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            CardDisplay display = newCard.GetComponent<CardDisplay>();
            display.card = card;
            display.UpdateCardDisplay();
            cardsInDraw.Add(newCard);
            newCard.SetActive(false);
        }
        ShuffleDrawPile();
        DrawHand();
    }
    void DrawHand()
    {
        for (int x = 0; x < handSize; x++)
        {
            if (cardsInDraw.Count == 0) RefillDrawPile();
            DrawCard();
        }
        UpdateHandVisual();
    }
    public void DrawCard()
    {
        cardsInHand.Add(cardsInDraw[0]);
        cardsInDraw[0].SetActive(true);
        cardsInDraw.Remove(cardsInDraw[0]);
    }
    public void DiscardCard(GameObject card)
    {
        cardsInDiscard.Add(card);
        card.SetActive(false);
        cardsInHand.Remove(card);
        UpdateHandVisual();
    }
    public void DiscardAll()
    {
        foreach(GameObject card in cardsInHand)
        {
            cardsInDiscard.Add(card);
            
            card.SetActive(false);
        }
        cardsInHand.Clear();
        DrawHand();
    }
    void RefillDrawPile()
    {
        foreach(GameObject card in cardsInDiscard)
        {
            cardsInDraw.Add(card);
        }
        cardsInDiscard.Clear();
        ShuffleDrawPile();
    }
    public void ShuffleDrawPile()
    {
        for (int i = cardsInDraw.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = cardsInDraw[i];
            cardsInDraw[i] = cardsInDraw[randomIndex];
            cardsInDraw[randomIndex] = temp;
        }
    }
    void UpdateHandVisual()
    {
        int cardCount = cardsInHand.Count;
        for(int i = 0; i < cardCount; i++)
        {
            float cardIndex = i - (cardCount - 1) / 2f;
            float normalisedPos = 2f * i/(cardCount - 1) - 1f;
            if(cardCount == 1) normalisedPos = 0f;
            cardsInHand[i].transform.rotation = Quaternion.Euler(0f, 0f, handFanning * cardIndex);
            cardsInHand[i].transform.localPosition = new Vector3(handSpacing * cardIndex, verticalSpacing * (1 - normalisedPos * normalisedPos), 0f);
        }
    }
}
