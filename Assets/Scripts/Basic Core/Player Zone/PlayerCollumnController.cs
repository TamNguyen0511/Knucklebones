using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CollumnIndex
{
    /// Compare to P1
    Left,
    Mid,
    Right
}
public class PlayerCollumnController : MonoBehaviour
{
    public CollumnIndex collumnIndex;
    public PlayerController player;

    public int totalCollumnScore;

    public List<PlayedDiceData> playedDices = new List<PlayedDiceData>();
    public bool isAddable;

    #region References UI
    public Text totalHoldingDiceTxt;
    #endregion

    private Button button;

    private void Start()
    {
        AddPlayerDiceView();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => AddToCollumn(player.currentTurnDice));
    }

    private void OnMouseEnter()
    {
        button.interactable = true;
    }
    private void OnMouseExit()
    {
        button.interactable = false;
    }
    private void AddPlayerDiceView()
    {
        for (int i = 0; i < 3; i++)
            playedDices.Add(transform.GetChild(i).GetComponent<PlayedDiceData>());
    }
    public void AddToCollumn(int numberToAdd)
    {
        if (playedDices.Count > 3)
        {
            Debug.Log("Can no longer add to this collumn");
            return;
        }

        if (!isAddable)
            return;

        for (int i = 0; i < playedDices.Count; i++)
        {
            if (!playedDices[i].isHoldingDice)
            {
                playedDices[i].holdingDiceNumber = numberToAdd;
                playedDices[i].isHoldingDice = true;
                playedDices[i].displayText.text = numberToAdd.ToString();
                break;
            }
        }

        DenyOppSameValueDice(numberToAdd);

        TotalHoldingDiceValue();

        player.totalFilledDice++;

        player.StateHandle(GameState.ChangeTurn);
    }

    private void TotalHoldingDiceValue()
    {
        int leftCData = playedDices[0].holdingDiceNumber;
        int midCData = playedDices[1].holdingDiceNumber;
        int rightCData = playedDices[2].holdingDiceNumber;

        totalCollumnScore = 0;

        /// Calculate correct total collumn's score
        if (leftCData == midCData && leftCData == rightCData)
            totalCollumnScore = (leftCData + midCData + rightCData) * 3;
        else if (leftCData == midCData)
            totalCollumnScore = (leftCData + midCData) * 2 + rightCData;
        else if (leftCData == rightCData)
            totalCollumnScore = (leftCData + rightCData) * 2 + midCData;
        else if (midCData == rightCData)
            totalCollumnScore = (midCData + rightCData) * 2 + leftCData;
        else totalCollumnScore = leftCData + midCData + rightCData;

        totalHoldingDiceTxt.text = totalCollumnScore.ToString();
    }

    private void DenyOppSameValueDice(int justAddedDice)
    {
        foreach (PlayerCollumnController playerCollumn in player.oppPlayer.playerCollumns)
        {
            if (playerCollumn.collumnIndex != this.collumnIndex)
                break;
            foreach (PlayedDiceData diceData in playerCollumn.playedDices)
            {
                if (diceData.holdingDiceNumber == justAddedDice)
                {
                    diceData.holdingDiceNumber = 0;
                    diceData.displayText.text = "";
                    diceData.isHoldingDice = false;
                    playerCollumn.TotalHoldingDiceValue();
                    playerCollumn.player.totalFilledDice--;
                    
                }
            }
        }
        player.oppPlayer.UpdateTotalPoint();
    }
}