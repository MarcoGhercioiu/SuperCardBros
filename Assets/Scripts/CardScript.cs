using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardScript : ScriptableObject
{

    public new string name;
    public int attack;
    public int mana;
    public int defense;

    public void Print(){
        Debug.Log(name + ", Attack: " + attack + ", Defense: " + defense + ", cost: " + mana);
    }


}
