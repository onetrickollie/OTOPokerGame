using System.Collections.Generic;
using UnityEngine;

public class BJManager1 : MonoBehaviour
{
    private List<int> cardDeck;

    public void InitializeDeck()
    {
        cardDeck = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 2; j <= 14; j++)
            {
                cardDeck.Add(j);
            }
        }
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < cardDeck.Count; i++)
        {
            int randomIndex = Random.Range(i, cardDeck.Count);
            (cardDeck[i], cardDeck[randomIndex]) = (cardDeck[randomIndex], cardDeck[i]);
        }
    }

    public int DrawCard()
    {
        if (cardDeck.Count == 0)
        {
            InitializeDeck();
        }

        int randomIndex = Random.Range(0, cardDeck.Count);
        int drawnCard = cardDeck[randomIndex];
        cardDeck.RemoveAt(randomIndex);

        Debug.Log("Drawn Card: " + drawnCard);
        return drawnCard;
    }

    public int GetDeckCount()
    {
        return cardDeck.Count;
    }
}
