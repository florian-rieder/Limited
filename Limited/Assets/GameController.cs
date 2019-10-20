using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public int currentTurn = 0;
    public void NextTurn(){
        //next turn code
        playerInventory.oil += 1;
        currentTurn++;
    }
}
