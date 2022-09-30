using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    StartOfGame,
    RollNewDice,
    AddDice,
    ChangeTurn,
    WaitInOppTurn,
    Endgame
}
public class GameController : SingletonMono<GameController>
{
    public PlayerController player1;
    public PlayerController player2;

    public GameObject p1InfoZone;
    public GameObject p2InfoZone;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        FindOpponent();
        SetPlayerStartState();
    }

    #region For init game
    private void FindOpponent()
    {
        player1.oppPlayer = player2;
        player2.oppPlayer = player1;
    }

    private void SetPlayerStartState()
    {
        int ran = Random.Range(0, 2);
        if (ran == 0)
        {
            player1.StateHandle(GameState.RollNewDice);
            player2.StateHandle(GameState.WaitInOppTurn);
        }
        else
        {
            player2.StateHandle(GameState.RollNewDice);
            player1.StateHandle(GameState.WaitInOppTurn);
        }
    }
    #endregion

    public void EndGame()
    {
        int p1Score = 0;
        int p2Score = 0;
        foreach (PlayerCollumnController p1Collumn in player1.playerCollumns)
            p1Score += p1Collumn.totalCollumnScore;
        foreach (PlayerCollumnController p2Collumn in player2.playerCollumns)
            p2Score += p2Collumn.totalCollumnScore;

        //Debug.Log("Player 1: " + p1Score + " - Player 2: " + p2Score);
        if (p1Score > p2Score)
            Debug.Log("Player 1 win");
        else if (p1Score < p2Score) Debug.Log("Player 2 win");
    }
}
