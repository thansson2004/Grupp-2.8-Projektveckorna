using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Timohty
public class BattleHUD : MonoBehaviour
{
  public Text nameText;
    public Slider hpSlider;

    public void SetHUD(Fighting unit)
    {
        nameText.text = unit.unitName; //Namn på fienden/spelare på hud
        hpSlider.maxValue = unit.maxHP;//Max väre på hp slider
        hpSlider.value = unit.currentHP;//Värde på hpslider
    }
    public void SetHP(int hp) //Ändrar värdet på hp slider beroende på hur mycket hp
    {
        hpSlider.value = hp;
    }
}
