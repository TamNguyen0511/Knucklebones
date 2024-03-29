﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public int finalSide = 0;

    // Array of dice sides sprites to load from Resources folder
    [SerializeField] private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    [SerializeField] private Image image;

    // Use this for initialization
    private void Start()
    {

        // Assign Renderer component
        image = GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        //diceSides = Resources.LoadAll<Sprite>("DiceSides/");
    }

    // If you left click over the dice then RollTheDice coroutine is started
    //private void OnMouseDown()
    //{
    //    StartCoroutine("RollTheDice");
    //}

    // Coroutine that rolls the dice
    public IEnumerator RollTheDice()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            image.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;

        // Show final dice value in Console
        //Debug.Log(finalSide);
    }
}
