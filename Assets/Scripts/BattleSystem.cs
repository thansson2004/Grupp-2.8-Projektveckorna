using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Timothy 
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }//Vilken State som man är i i fighting scenen
public enum EnemyState { Attack, HEAL, ENEMYTURN } //Vad datorn kommer göra
public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;   //Prefab
    public GameObject enemyPrefab;

    public Transform playerBattlestation; //Spawn punkter för Spelaren o Fienden
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
        state = BattleState.START;  //Sätter game staten till Start
        StartCoroutine(SetupBattle()); //Startar koden till det som händer precis innan fighten
    }

    IEnumerator SetupBattle() //Allt som händer innan spelaren ska attackera första gången
    {
      GameObject playerGO = Instantiate(playerPrefab, playerBattlestation); //Instantiatar spelaren på rätt position
        playerUnit = playerGO.GetComponent<Fighting>();
      GameObject enemyGo = Instantiate(enemyPrefab, enemyBattlestation);  //finede på rätt position
        enemyUnit = enemyGo.GetComponent<Fighting>();

        dialog.text = enemyUnit.unitName + " Attackerar dig!"; //Ändrar narrator texten 
        playerHUD.SetHUD(playerUnit); //Ändrar HUD så att den har spelarens/fiendens namn
        enemyHUD.SetHUD(enemyUnit);
        state = BattleState.PLAYERTURN; //Ändrar till spelarens hur
        yield return new WaitForSeconds(2f); //Väntar 2 sekunder innan den byter till player turn funktionen
        playerturn();
    }
    public void playerturn()
    {
        dialog.text = "Välj en aktion"; //Säger tilll dig att du kan välja en funktion genom att trycka på kanpparna
    }
    IEnumerator playerAttack() //Koden om spelaren väljer att attackera och göra skada
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage); //Hämtar damage funktionen från fighting koden
        enemyHUD.SetHP(enemyUnit.currentHP); //Ändrar hp sliden på HUD
        dialog.text = "Attacken lyckades!"; //Narrator
        yield return new WaitForSeconds(2f);
        if (isDead)     //Kollar genom fighting scripten om fienden har dött
        {
            state = BattleState.WON;    //Om fineden är död så ändras staten till att du vinner
            StartCoroutine(EndBattle()); //Fighten avslutas
        }
        else
        {
            state = BattleState.ENEMYTURN; //Om fienden inte är död så byts det till fiendens tur
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator playerHeal() //Spelaren väljer att Hela sig själv
    {
        playerUnit.Heal(15); //Använder koden för att hela från fighting scripten
        playerHUD.SetHP(playerUnit.currentHP);//Ändra hp sliden i HUD
        dialog.text = "Du äter en bit ost och känner dina krafter komma tillbaka..."; //Narrator
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN; //Ändrar till fiendens tur
        StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {

        if (enemyUnit.currentHP < 5) //Kollar om fienden är låg på hp
        {
            enemyState = EnemyState.HEAL; //om låg hp fienden använder heal funktionen
        } else
        {
            enemyState = EnemyState.Attack; //Om hög hp attackerar
        }
    
        if(enemyState == EnemyState.Attack)  //Fienden attackerar
        {
            dialog.text = enemyUnit.unitName + " attackerar!";
            yield return new WaitForSeconds(1f);
            bool isDead = playerUnit.TakeDamage(enemyUnit.damage); //Använder samma kod som spelarens attack, tagen från fighting koden
            playerHUD.SetHP(playerUnit.currentHP); //Ändrar hp slidern
            yield return new WaitForSeconds(1f); 
            if (isDead) //Kollar om spelaren är död
            {
                state = BattleState.LOST;  //Du förlorar
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN; //Ändrar till spelarens tur
                playerturn();
            }
        } else if(enemyState == EnemyState.HEAL) //Om fienden helar
        {
            enemyUnit.Heal(20); //lägger till 20 hp på fiendens nuvarande hp genom heal funktionen i fighting koden
            enemyHUD.SetHP(playerUnit.currentHP);
            dialog.text = "Enemy healed it self";  
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN; //Ändrar till spelarens tur
           
        }
       
        
        
     
    }
    IEnumerator EndBattle() //Funktion för att avsluta striden
    {
        if(state == BattleState.WON) //Kollar om du vunnit 
        {
            dialog.text = "Du besegrade " + enemyUnit.unitName + "!!!"; //Monolog
            yield return new WaitForSeconds(2f);
          
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Byter tilll nästa scen i buildindexen
        }
        else if (state == BattleState.LOST) //kollar om du förlorade
        {
            dialog.text = "O nej, du förlorade!";  
            yield return new WaitForSeconds(2f); 
            SceneManager.LoadScene("GameOver");  //Laddar game over scene
        }
    }
    public void OnAttackButton() //När spelaren trycker på knappen för att attackera 
    {
        if (state != BattleState.PLAYERTURN) return; //Kollar så att det är spelarens tur
        StartCoroutine(playerAttack()); //Startar attack funktion
    }
    public void OnHealButton() //När spelaren trycker på knappen för att hela
    {
        if (state != BattleState.PLAYERTURN) return; //Kollar så att det är spelarens tur
        StartCoroutine(playerHeal());//Startar Heal funktion
    }
    public void OnBuffButton()  //När spelaren trycker på knappen för att buffa attack
    {
        if (state != BattleState.PLAYERTURN) return;  //Kollar så att det är spelarens tur
        StartCoroutine(playerBuff()); //Startar buff funktion
    }
    IEnumerator playerBuff() //Funktion för att buffa spelarens attack(Öka skadan spelaren gör)
    {
        playerUnit.Buff(25); //Tar funktion från fighting scripten lägger till antal skada jag vill öka
        dialog.text = "Du ringer ditt ex"; //Episk förklaring till varför du ökar skada
        yield return new WaitForSeconds(1f);
        dialog.text = "Du blir arg av att höra hennes röst";
        yield return new WaitForSeconds(1f);
        dialog.text = "Attack ökas med ett steg...";
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN; //byter till fiendens tur
        StartCoroutine(EnemyTurn());
    }
}
