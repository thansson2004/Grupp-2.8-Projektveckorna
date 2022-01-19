using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattlestation;
    public Transform enemyBattlestation;
    
    public BattleState state;
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        Instantiate(playerPrefab, playerBattlestation);
        Instantiate(enemyPrefab, enemyBattlestation);
    }

  
}
