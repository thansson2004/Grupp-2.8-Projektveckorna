using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighting : MonoBehaviour
{
    public int damage;
    public int maxHP;
    public int currentHP;
    public string unitName;
//Timothy
    public bool TakeDamage(int dmg) //Gör så att man tar skada
    {
        currentHP -= dmg;

        if (currentHP <= 0) return true;  //Kollar om man är död
        else return false;
    }
    public void Heal(int amount)   //Helar dig
    {
        currentHP += amount; 
        if(currentHP >= maxHP) //Ser till så att man inte kan hela mer än sitt max hp
        {
            currentHP = maxHP;
        }
    }
    public void Buff(int amount)  //Höjer skadan man kan göra
    {
        damage += amount;
    }
}
