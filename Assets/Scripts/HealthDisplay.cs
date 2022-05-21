using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
[System.Serializable]

public class HealthDisplay : MonoBehaviour
{
	// public Player player;
	// public int currHealth;

	public TextMeshProUGUI healthText;

	public void updateHealth(int health) {
        healthText.text = health.ToString();
    }
}
