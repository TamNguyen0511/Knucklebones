using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int currentTurnDice;
    public int totalFilledDice;

    public PlayerController oppPlayer;

    public GameState playerCurrentState;
    public List<PlayerCollumnController> playerCollumns = new List<PlayerCollumnController>();

    #region Properties UI
    public Text displayDiceNumber;
    #endregion

    private Color fadedColor = new Color(1, 1, 1, 0);
    private Color showColor = new Color(1, 1, 1, 1);

    public int RollNewDice()
    {
        return Random.Range(1, 7);
    }

    public void StateHandle(GameState newState)
    {
        if (playerCurrentState == newState) return;
        playerCurrentState = newState;
        switch (playerCurrentState)
        {
            case GameState.RollNewDice:
                displayDiceNumber.transform.parent.GetComponent<Image>().color = showColor;
                currentTurnDice = RollNewDice();
                displayDiceNumber.text = currentTurnDice.ToString();
                StateHandle(GameState.AddDice);
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
                displayDiceNumber.transform.parent.GetComponent<Image>().color = fadedColor;
                foreach (PlayerCollumnController playerCollumn in playerCollumns)
                {
                    playerCollumn.isAddable = false;
                    playerCollumn.GetComponent<Button>().interactable = false;
                }
                break;
            case GameState.Endgame:
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
}
