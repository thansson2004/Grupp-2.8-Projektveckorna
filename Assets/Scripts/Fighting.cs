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
    public bool TakeDamage(int dmg) //G�r s� att man tar skada
    {
        currentHP -= dmg;

        if (currentHP <= 0) return true;  //Kollar om man �r d�d
        else return false;
    }
    public void Heal(int amount)   //Helar dig
    {
        currentHP += amount; 
        if(currentHP >= maxHP) //Ser till s� att man inte kan hela mer �n sitt max hp
        {
            currentHP = maxHP;
        }
    }
    public void Buff(int amount)  //H�jer skadan man kan g�ra
    {
        damage += amount;
    }
}
