using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    int ai = 1;
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

        dialog.text = enemyUnit.unitName + " Attackerar dig!";
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
            StartCoroutine(EndBattle());
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
        ai = Random.Range(1, 4);
        dialog.text = enemyUnit.unitName + " attackerar!";
        yield return new WaitForSeconds(1f);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1f);
        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            playerturn();
        }
    }
    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialog.text = "Du besegrade " + enemyUnit.unitName + "!!!";
            yield return new WaitForSeconds(2f);
          
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (state == BattleState.LOST)
        {
            dialog.text = "O nej, du förlorade!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("GameOver");
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
    public void OnBuffButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(playerBuff());
    }
    IEnumerator playerBuff()
    {
        playerUnit.Buff(25);
        dialog.text = "Du ringer ditt ex";
        yield return new WaitForSeconds(1f);
        dialog.text = "Du blir arg av att höra hennes röst";
        yield return new WaitForSeconds(1f);
        dialog.text = "Attack ökas med ett steg...";
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
}
