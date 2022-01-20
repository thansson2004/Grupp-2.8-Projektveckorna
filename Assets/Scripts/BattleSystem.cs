using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattlestation;
    public Transform enemyBattlestation;

    Fighting playerUnit;
    Fighting enemyUnit;

    public Text dialog;
    public BattleState state;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
      GameObject playerGO = Instantiate(playerPrefab, playerBattlestation);
        playerUnit = playerGO.GetComponent<Fighting>();
      GameObject enemyGo = Instantiate(enemyPrefab, enemyBattlestation);
        enemyUnit = enemyGo.GetComponent<Fighting>();

        dialog.text = "En " + enemyUnit.unitName + " Attackerar dig!";
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        state = BattleState.PLAYERTURN;
        yield return new WaitForSeconds(2f);
        playerturn();
    }
    public void playerturn()
    {
        dialog.text = "Välj en aktion";
    }
    IEnumerator playerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialog.text = "Attacken lyckades!";
        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator playerHeal()
    {
        playerUnit.Heal(15);
        playerHUD.SetHP(playerUnit.currentHP);
        dialog.text = "Du äter en bit ost och känner dina krafter komma tillbaka...";
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {
        dialog.text = enemyUnit.unitName + " attackerar!";
        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }else
        {
            state = BattleState.PLAYERTURN;
            playerturn();
        }
    }
    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialog.text = "Du besegrade " + enemyUnit.unitName + "!!!";
        }
        else if (state == BattleState.LOST)
        {
            dialog.text = "O nej, du förlorade!";
        }
    }
    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(playerAttack());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(playerHeal());
    }

}
