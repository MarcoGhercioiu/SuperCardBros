using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card : MonoBehaviour
{
	public CardID id;
	public Rarity rarity;
	public string name;
	public int health;
	public int attack;
	public int manaCost;
    public Passive passive;
    public string passiveText;
    public Texture2D texture;
	
	public int positionInHand;
	public bool currentlyHovered;
	public bool currentlySelected;
	public BoardSpace selectedBoardSpace; 
	
	public CardDisplay cardDisplay;
	
	public GameObject CardInHandObject;
	public GameObject CardObject;

	public void init(GameObject cardObject, CardID CardId, CardConfig config) {
        this.CardObject = cardObject;
        this.id = CardId;
        this.rarity = config.rarity;
        this.name = config.name;
        this.health = config.health;
        this.attack = config.attack;
        this.manaCost = config.manaCost;
        this.texture = config.texture;
        this.passive = config.passive;
        this.passiveText = CardDatabase.PassiveDescriptions[this.passive];

		currentlyHovered = false;
		currentlySelected = false;

		cardDisplay = CardInHandObject.GetComponent<CardDisplay>() as CardDisplay;
		cardDisplay.init(this);

	}
	
	public void enableHandPrefab() {
        cardDisplay.CardPrefab.SetActive(true);
    }
	
	public void disableHandPrefab() {
        cardDisplay.CardPrefab.SetActive(false);
    }

	public void updatePosition(int pos) {
		positionInHand = pos;
	}
	
	public void updateDisplay(int totalCards) {
		cardDisplay.updateDisplay(totalCards);
	}

}