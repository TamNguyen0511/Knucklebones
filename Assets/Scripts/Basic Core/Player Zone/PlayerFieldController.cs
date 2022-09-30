using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFieldController : SingletonMono<PlayerFieldController>
{
    public List<PlayerCollumnController> playedDices = new List<PlayerCollumnController>();

    public int RollNewDice()
    {
        return Random.Range(1, 7);
    }

    public void SetNewDice()
    {

    }
}
