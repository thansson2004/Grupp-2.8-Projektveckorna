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
        nameText.text = unit.unitName; //Namn p� fienden/spelare p� hud
        hpSlider.maxValue = unit.maxHP;//Max v�re p� hp slider
        hpSlider.value = unit.currentHP;//V�rde p� hpslider
    }
    public void SetHP(int hp) //�ndrar v�rdet p� hp slider beroende p� hur mycket hp
    {
        hpSlider.value = hp;
    }
}
