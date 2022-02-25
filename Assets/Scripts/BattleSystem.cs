using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Timothy 
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }//Vilken State som man �r i i fighting scenen
public enum EnemyState { Attack, HEAL, ENEMYTURN } //Vad datorn kommer g�ra
public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;   //Prefab
    public GameObject enemyPrefab;

    public Transform playerBattlestation; //Spawn punkter f�r Spelaren o Fienden
    public Transform enemyBattlestation;

    Fighting playerUnit;
    Fighting enemyUnit;

    public Text dialog;
    public BattleState state;
    public EnemyState enemyState;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
 
    void Start()
    {
        state = BattleState.START;  //S�tter game staten till Start
        StartCoroutine(SetupBattle()); //Startar koden till det som h�nder precis innan fighten
    }

    IEnumerator SetupBattle() //Allt som h�nder innan spelaren ska attackera f�rsta g�ngen
    {
      GameObject playerGO = Instantiate(playerPrefab, playerBattlestation); //Instantiatar spelaren p� r�tt position
        playerUnit = playerGO.GetComponent<Fighting>();
      GameObject enemyGo = Instantiate(enemyPrefab, enemyBattlestation);  //finede p� r�tt position
        enemyUnit = enemyGo.GetComponent<Fighting>();

        dialog.text = enemyUnit.unitName + " Attackerar dig!"; //�ndrar narrator texten 
        playerHUD.SetHUD(playerUnit); //�ndrar HUD s� att den har spelarens/fiendens namn
        enemyHUD.SetHUD(enemyUnit);
        state = BattleState.PLAYERTURN; //�ndrar till spelarens hur
        yield return new WaitForSeconds(2f); //V�ntar 2 sekunder innan den byter till player turn funktionen
        playerturn();
    }
    public void playerturn()
    {
        dialog.text = "V�lj en aktion"; //S�ger tilll dig att du kan v�lja en funktion genom att trycka p� kanpparna
    }
    IEnumerator playerAttack() //Koden om spelaren v�ljer att attackera och g�ra skada
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage); //H�mtar damage funktionen fr�n fighting koden
        enemyHUD.SetHP(enemyUnit.currentHP); //�ndrar hp sliden p� HUD
        dialog.text = "Attacken lyckades!"; //Narrator
        yield return new WaitForSeconds(2f);
        if (isDead)     //Kollar genom fighting scripten om fienden har d�tt
        {
            state = BattleState.WON;    //Om fineden �r d�d s� �ndras staten till att du vinner
            StartCoroutine(EndBattle()); //Fighten avslutas
        }
        else
        {
            state = BattleState.ENEMYTURN; //Om fienden inte �r d�d s� byts det till fiendens tur
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator playerHeal() //Spelaren v�ljer att Hela sig sj�lv
    {
        playerUnit.Heal(15); //Anv�nder koden f�r att hela fr�n fighting scripten
        playerHUD.SetHP(playerUnit.currentHP);//�ndra hp sliden i HUD
        dialog.text = "Du �ter en bit ost och k�nner dina krafter komma tillbaka..."; //Narrator
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN; //�ndrar till fiendens tur
        StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {

        if (enemyUnit.currentHP < 5) //Kollar om fienden �r l�g p� hp
        {
            enemyState = EnemyState.HEAL; //om l�g hp fienden anv�nder heal funktionen
        } else
        {
            enemyState = EnemyState.Attack; //Om h�g hp attackerar
        }
    
        if(enemyState == EnemyState.Attack)  //Fienden attackerar
        {
            dialog.text = enemyUnit.unitName + " attackerar!";
            yield return new WaitForSeconds(1f);
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage); //Anv�nder samma kod som spelarens attack, tagen fr�n fighting koden
            playerHUD.SetHP(playerUnit.currentHP); //�ndrar hp slidern
            yield return new WaitForSeconds(1f); 
            if (isDead) //Kollar om spelaren �r d�d
            {
                state = BattleState.LOST;  //Du f�rlorar
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN; //�ndrar till spelarens tur
                playerturn();
            }
        } else if(enemyState == EnemyState.HEAL) //Om fienden helar
        {
            enemyUnit.Heal(20); //l�gger till 20 hp p� fiendens nuvarande hp genom heal funktionen i fighting koden
            enemyHUD.SetHP(playerUnit.currentHP);
            dialog.text = "Enemy healed it self";  
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN; //�ndrar till spelarens tur
           
        }
       
        
        
     
    }
    IEnumerator EndBattle() //Funktion f�r att avsluta striden
    {
        if(state == BattleState.WON) //Kollar om du vunnit 
        {
            dialog.text = "Du besegrade " + enemyUnit.unitName + "!!!"; //Monolog
            yield return new WaitForSeconds(2f);
          
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Byter tilll n�sta scen i buildindexen
        }
        else if (state == BattleState.LOST) //kollar om du f�rlorade
        {
            dialog.text = "O nej, du f�rlorade!";  
            yield return new WaitForSeconds(2f); 
            SceneManager.LoadScene("GameOver");  //Laddar game over scene
        }
    }
    public void OnAttackButton() //N�r spelaren trycker p� knappen f�r att attackera 
    {
        if (state != BattleState.PLAYERTURN) return; //Kollar s� att det �r spelarens tur
        StartCoroutine(playerAttack()); //Startar attack funktion
    }
    public void OnHealButton() //N�r spelaren trycker p� knappen f�r att hela
    {
        if (state != BattleState.PLAYERTURN) return; //Kollar s� att det �r spelarens tur
        StartCoroutine(playerHeal());//Startar Heal funktion
    }
    public void OnBuffButton()  //N�r spelaren trycker p� knappen f�r att buffa attack
    {
        if (state != BattleState.PLAYERTURN) return;  //Kollar s� att det �r spelarens tur
        StartCoroutine(playerBuff()); //Startar buff funktion
    }
    IEnumerator playerBuff() //Funktion f�r att buffa spelarens attack(�ka skadan spelaren g�r)
    {
        playerUnit.Buff(25); //Tar funktion fr�n fighting scripten l�gger till antal skada jag vill �ka
        dialog.text = "Du ringer ditt ex"; //Episk f�rklaring till varf�r du �kar skada
        yield return new WaitForSeconds(1f);
        dialog.text = "Du blir arg av att h�ra hennes r�st";
        yield return new WaitForSeconds(1f);
        dialog.text = "Attack �kas med ett steg...";
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN; //byter till fiendens tur
        StartCoroutine(EnemyTurn());
    }
}
