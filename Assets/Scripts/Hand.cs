using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Hand : MonoBehaviour
{
    public static readonly int HandSize = 5;
	public GameObject CardPrefab;

	public Card[] cards;

	public bool displayingHand;
	public bool displayingSelected;
	public int currentlyHovered;
	public int currentlySelected;

	public GameObject HandObject;

	public void init(Deck deck) {
		this.reset();
		this.fillHand(deck);
		this.resetDisplay();
	}

	public void reset() {
		displayingHand = false;
		displayingSelected = false;
		currentlyHovered = 0;
		currentlySelected = -1;
		cards = new Card[HandSize];
		Card[] cardObjects = HandObject.GetComponentsInChildren<Card>();
		for(int i = 0; i < cardObjects.Length; i++) {
			cardObjects[i].CardObject.transform.parent = null;
			Destroy(cardObjects[i].CardObject);
		}
	}

    public void draw(Deck deck)
    {
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] == null) {
                Card drawnCard = deck.draw();
				if(!addCardToHand(drawnCard)) {
					Debug.Log("Couldn't add card");
				}
				drawnCard.enableHandPrefab();
				return;
			}
		}
		Debug.Log("Hand is full.");
	}

	private void fillHand(Deck deck) {
        for (int i = 0; i < Config.StartingHandSize; i++)
        {
			if(!isHandFull()) {
                Card drawnCard = deck.draw();
				if(!addCardToHand(drawnCard)) {
					Debug.Log("Couldn't add card " + drawnCard.name + " to hand.");
				}
			}
		}
	}

	public Card getCard(int cardNum) {
		try {
			return cards[cardNum];
		}
		catch {
			Debug.Log("Out of range card");
			return null;
		}
	}

	public bool isHandFull() {
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] == null) {
				return false;
			}
		}
		return true;
	}

	private bool isHandScrollable() {
		int counter = 0;
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] != null) {
				counter++;
			}
		}
		return counter >= 2;
	}

	private bool addCardToHand(Card card) {
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] == null) {
				cards[i] = card;
				this.setCardTransformToHand(card);
				return true;
			}
		}
		return false;
	}

	private void setCardTransformToHand(Card card) {
		if(card != null) {
			card.CardObject.transform.parent = HandObject.transform;
			card.CardObject.transform.localPosition = Vector3.zero;
			card.CardObject.transform.localRotation = Quaternion.identity;
			card.CardObject.transform.localScale = new Vector3(UIConstants.CardInHandDefaultSize, UIConstants.CardInHandDefaultSize, UIConstants.CardInHandDefaultSize);
		}
		else {
			Debug.Log("Card reference is null.");
		}
	}

	public int getNumCardsInHand() {
		int counter = 0;
		for(int i = 0; i < HandSize;  i++) {
			if(cards[i] != null) {
				counter++;
			}
		}
		return counter;
	}

	private void removeCardFromHand(int cardIndex) {
		if(cards[cardIndex] != null) {
			cards[cardIndex] = null;
			for(int i = cardIndex; i < HandSize - 1;  i++) {
				if(cards[i + 1] != null) {
					cards[i] = cards[i + 1];
					cards[i + 1] = null;
				}
				else {
					break;
				}
			}
		}
	}

	public void resetDisplay() {
		this.resetHover();
		this.deselectCard();
		this.hide();
	}

	public void display() {
		displayingHand = true;
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] != null) {
				Card card = cards[i];
				card.enableHandPrefab();
				card.CardObject.transform.localPosition = Vector3.one;
				card.CardObject.transform.localRotation = Quaternion.identity;
				card.CardObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			}
		}
	}

	public void hide(){
		displayingHand = false;
		this.deselectCard();
		this.resetHover();
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] != null) {
				cards[i].disableHandPrefab();
			}
		}
	}

	public void playCard(Board board) {
		board.addCard(cards[currentlySelected], cards[currentlySelected].selectedBoardSpace);
		this.removeCardFromHand(currentlySelected);
		this.deselectCard();
		this.resetHover();
	}

	public void selectCard(int row, Board board) {
        if (this.getNumCardsInHand() > 0 && currentlyHovered > -1 && cards[currentlyHovered] != null && board.getFirstAvailableSpace(row) != null)
        {
			displayingSelected = true;
            currentlySelected = currentlyHovered;
            cards[currentlySelected].currentlySelected = true;
			cards[currentlySelected].selectedBoardSpace = board.getFirstAvailableSpace(row);
		}
	}

	public void deselectCard() {
		if(currentlySelected != -1) {
			if(cards[currentlySelected] != null) {
				cards[currentlySelected].currentlySelected = false;
				this.setCardTransformToHand(cards[currentlySelected]);
			}
			currentlyHovered = currentlySelected;
			this.updateHover();
		}
		else {
			this.resetHover();
		}
		displayingSelected = false;
		currentlySelected = -1;
	}

	public void moveSelected(Direction d, Board board) {
		int row = 0;
		BoardSpace newBoardSpace;
		if(board.getOpenSpaces(row) <= 1) {
			Debug.Log("No open board spaces to move to.");
		}
		else if(d == Direction.Left) {
			newBoardSpace = board.getFirstSpaceLeft(row, cards[currentlySelected].selectedBoardSpace);
			if(newBoardSpace != null) {
				cards[currentlySelected].selectedBoardSpace = newBoardSpace;
			}
			else {
				Debug.Log("Board returned a null board space, not moving selection.");
			}
		}
		else if(d == Direction.Right) {
			newBoardSpace = board.getFirstSpaceRight(row, cards[currentlySelected].selectedBoardSpace);
			if(newBoardSpace != null) {
				cards[currentlySelected].selectedBoardSpace = newBoardSpace;
			}
			else {
				Debug.Log("Board returned a null board space, not moving selection.");
			}
		}
		else {
			Debug.Log("Unrecognized direction");
		}
	}

    public void hoverCard(Direction d) {
		int newHovered;
		if(!isHandScrollable()) {
			Debug.Log("Not enough cards in hand to scroll");
			return;
		}
		else if(d == Direction.Left) {
			cards[currentlyHovered].currentlyHovered = false;
			newHovered = currentlyHovered - 1;
			if(newHovered < 0) {
				newHovered = HandSize - 1;
			}
			while(cards[newHovered] == null) {
				newHovered = newHovered - 1;
				if(newHovered < 0) {
					newHovered = HandSize - 1;
				}
			}
			currentlyHovered = newHovered;
		}
		else if(d == Direction.Right) {
			cards[currentlyHovered].currentlyHovered = false;
			newHovered = currentlyHovered + 1;
			if(newHovered >= HandSize) {
				newHovered = 0;
			}
			while(isHandScrollable() && cards[newHovered] == null) {
				newHovered = newHovered + 1;
				if(newHovered >= HandSize) {
					newHovered = 0;
				}
			}
			currentlyHovered = newHovered;
		}
		else {
			Debug.Log("Unrecognized direction");
		}
		this.updateHover();
    }

	public void updateHover() {
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] != null) {
				cards[i].currentlyHovered = false;
			}
		}
		if(currentlyHovered >= 0 && currentlyHovered < Config.HandSize && cards[currentlyHovered] != null) {
			cards[currentlyHovered].currentlyHovered = true;
		}
	}

	public void resetHover() {
		currentlyHovered = -1;
		for(int i = 0; i < HandSize; i++) {
			if(cards[i] != null) {
				currentlyHovered = i;
				this.updateHover();
				return;
			}
		}
		this.updateHover();
	}

	void Update() {
		if(displayingHand) {
			int cardsInHand = this.getNumCardsInHand();
			for(int i = 0; i < HandSize; i++) {
				if(cards[i] != null) {
					if(currentlyHovered == i) {
						cards[i].currentlyHovered = true;
					}
					if(currentlySelected == i) {
						cards[i].currentlySelected = true;
						cards[i].currentlyHovered = false;
					}
					cards[i].updatePosition(i);
					cards[i].updateDisplay(cardsInHand);
				}
			}
		}
	}
}
