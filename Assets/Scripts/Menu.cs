using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool gameStarted = false;
    [SerializeField]
    private GameManager _gm;

    //Start the game and disable the menu
    public void StartGame()
    {
        gameStarted = true;
        Debug.Log("Game Started");
        _gm.StartGame();
        this.gameObject.SetActive(false);
    }

}
