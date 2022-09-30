using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayedDiceData : MonoBehaviour
{
    public bool isHoldingDice;
    public int holdingDiceNumber;
    public Text displayText;

    private void OnEnable()
    {
        displayText = transform.GetChild(0).GetComponent<Text>();
        displayText.text = "";
    }
}
