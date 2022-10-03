using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int currentTurnDice;
    public int totalFilledDice;
    public Dice dice;

    public PlayerController oppPlayer;

    public GameState playerCurrentState;
    public List<PlayerCollumnController> playerCollumns = new List<PlayerCollumnController>();

    #region Properties UI
    public Text displayDiceNumber;
    public Text playerStateTxt;
    public Text playerTotalPointTxt;
    //public Text displayDiceNumber;
    #endregion

    private Color fadedColor = new Color(1, 1, 1, 0);
    private Color showColor = new Color(1, 1, 1, 1);

    public IEnumerator RollNewDice()
    {
        yield return StartCoroutine(dice.RollTheDice());
        currentTurnDice = dice.finalSide;
        displayDiceNumber.text = currentTurnDice.ToString();
        StateHandle(GameState.AddDice);
    }

    public void StateHandle(GameState newState)
    {
        if (playerCurrentState == newState) return;
        playerCurrentState = newState;
        playerStateTxt.text = playerCurrentState.ToString();
        switch (playerCurrentState)
        {
            case GameState.RollNewDice:
                displayDiceNumber.transform.parent.GetComponent<Image>().color = showColor;
                StartCoroutine(RollNewDice());
                break;
            case GameState.AddDice:
                foreach (PlayerCollumnController playerCollumn in playerCollumns)
                {
                    playerCollumn.isAddable = true;
                    playerCollumn.GetComponent<Button>().interactable = true;
                }
                break;
            case GameState.ChangeTurn:
                CheckEndGame();
                StateHandle(GameState.WaitInOppTurn);
                oppPlayer.StateHandle(GameState.RollNewDice);
                break;
            case GameState.WaitInOppTurn:
                UpdateTotalPoint();
                displayDiceNumber.transform.parent.GetComponent<Image>().color = fadedColor;
                foreach (PlayerCollumnController playerCollumn in playerCollumns)
                {
                    playerCollumn.isAddable = false;
                    playerCollumn.GetComponent<Button>().interactable = false;
                }
                break;
            case GameState.Endgame:
                foreach (PlayerCollumnController playerCollumn in playerCollumns)
                {
                    playerCollumn.isAddable = false;
                    playerCollumn.GetComponent<Button>().interactable = false;
                }
                GameController.Instance.EndGame();
                break;
        }
    }
    private void CheckEndGame()
    {
        if (totalFilledDice < 9)
            return;
        StateHandle(GameState.Endgame);
        oppPlayer.StateHandle(GameState.Endgame);
    }

    public void UpdateTotalPoint()
    {

        int currentTotalPoint = 0;
        foreach (PlayerCollumnController pCollum in playerCollumns)
            currentTotalPoint += pCollum.totalCollumnScore;

        playerTotalPointTxt.text = currentTotalPoint.ToString();
    }
}
