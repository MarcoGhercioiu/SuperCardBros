using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class CardDisplay : MonoBehaviour
{
    
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI attackText;
	public TextMeshProUGUI manaText;
    public TextMeshProUGUI descriptionText;
    public GameObject CardPrefab;
    public GameObject CardPrefabImage;
	public Card card;

    public void init(Card c) {
        card = c;
        this.addHandPrefab();
    }
    
	public void addHandPrefab() {
		this.updateDisplay();
        this.updateRarity();
        Sprite temp = Sprite.Create(card.texture, new Rect(0, 0, card.texture.width, card.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        CardPrefabImage.GetComponent<Image>().sprite = temp;
        CardPrefab.transform.SetParent(card.CardObject.transform);
        CardPrefab.transform.SetParent(card.CardObject.transform);
        CardPrefab.transform.localPosition = Vector3.zero;
        CardPrefab.SetActive(false);
    }

    public void updateDisplay() {
        nameText.text = card.name.ToString();
        healthText.text = card.health.ToString();
        attackText.text = card.attack.ToString();
        manaText.text = card.manaCost.ToString();
        descriptionText.text = card.passiveText;
    }

    public void updateRarity() {
        switch(card.rarity) {
            case Rarity.Common:
                CardPrefab.GetComponent<Image>().color = new Color32(180, 240, 255, 255);
                break;
            case Rarity.Uncommon:
                CardPrefab.GetComponent<Image>().color = new Color32(110, 170, 255, 255);
                break;
            case Rarity.Rare:
                CardPrefab.GetComponent<Image>().color = new Color32(240, 86, 255, 255);
                break;
            case Rarity.Epic:
                CardPrefab.GetComponent<Image>().color = new Color32(244, 32, 17, 255);
                break;
            case Rarity.Legendary:
                CardPrefab.GetComponent<Image>().color = new Color32(255, 187, 30, 255);
                break;
            default:
                CardPrefab.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
        }
    }
    
    public void updatePosition(int totalCards) {
        int handWidth = (UIConstants.CardWidth * totalCards) + UIConstants.HandPadding;
        int handLeftBorder = UIConstants.LeftBorder + ((UIConstants.FullHandWidth - handWidth) / 2);
        float cardX = handLeftBorder + (card.positionInHand * (UIConstants.CardWidth + UIConstants.CardPadding)) + (UIConstants.CardPadding / 2);
        card.transform.localPosition = new Vector3(cardX, UIConstants.PlayerHandVerticalOffset, 0);
    }

    public void updateDisplay(int totalCards) {
        if(card.currentlySelected) {
            this.updatePosition(totalCards);
            card.transform.parent = card.selectedBoardSpace.boardSpaceObject.transform;
            card.CardObject.transform.localScale = new Vector3(UIConstants.SelectedCardOnBoardSize, UIConstants.SelectedCardOnBoardSize, UIConstants.SelectedCardOnBoardSize);
            card.CardObject.transform.localPosition = new Vector3(0, 0, UIConstants.SelectedCardOnBoardZOffset);
            card.CardObject.transform.localRotation = Quaternion.identity;
        }
        else if(card.currentlyHovered) {
            this.updatePosition(totalCards);
            card.transform.localScale = new Vector3(UIConstants.CardInHandHoveredSize, UIConstants.CardInHandHoveredSize, UIConstants.CardInHandHoveredSize);
        }
        else {
            card.transform.localScale = new Vector3(UIConstants.CardInHandDefaultSize, UIConstants.CardInHandDefaultSize, UIConstants.CardInHandDefaultSize);
            this.updatePosition(totalCards);
        }
    }
}