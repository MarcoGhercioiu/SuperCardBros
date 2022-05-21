using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Deck : MonoBehaviour
{
	public Card[] deck;
	public GameObject DeckObject;
    public GameObject DeckPrefab;
	public GameObject CardPrefab;

    public void init() {
		this.reset();
		this.fillDeck();
    }

	public void reset() {
		this.deck = new Card[Config.DeckSize];
		Card[] cardObjects = DeckObject.GetComponentsInChildren<Card>();
		for(int i = 0; i < cardObjects.Length; i++) {
			cardObjects[i].CardObject.transform.parent = null;
			Destroy(cardObjects[i].CardObject);
		}
	}

	public void fillDeck() {
        for (int i = 0; i < Config.DeckSize; i++)
        {
			Card newCard = CardDatabase.generateRandomCard(CardPrefab);
			newCard.CardObject.transform.parent = DeckObject.transform;
			newCard.CardObject.transform.localPosition = new Vector3(0, 0, 0);
			deck[i] = newCard;
		}
	}

	public Card draw() {
        for (int i = 0; i < Config.DeckSize; i++)
        {
            if (deck[i] != null)
            {
				Card drawCard = deck[i];
				deck[i] = null;
				return drawCard;
			}
		}
		Debug.Log("Deck out of cards, unable to draw.");
		return null;
	}
}