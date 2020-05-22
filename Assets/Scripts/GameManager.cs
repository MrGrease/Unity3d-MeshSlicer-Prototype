using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private knife _knife;
    [SerializeField]
    private Sliceable _sliceable;
    [SerializeField]
    private Transition _trans;


    // Start is called before the first frame update
    void Start()
    {   //Randomly selects a shape from the avaliable ones
        _sliceable.SelectShape();
    }
    // Update is called once per frame
    void Update()
    {   //Ends the game once the sliceable moves out of the range of the knife 
        if(_sliceable.transform.position.x > 2.5f)
        {
            Debug.Log("Game Ended");
            EndGame();
        }
        //Quid the game if the back button is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }
    //Starts the game and lets the knife and sliceable know
    public void  StartGame()
    {
        _knife.SetGameStarted(true);
        _sliceable.SetGameStarted(true);
    }
    //Ends the game and starts the reload scene coroutine
    public void EndGame()
    {
        _knife.SetGameStarted(false);
        _sliceable.SetGameStarted(false);
        _trans.StartAnim();
        StartCoroutine("ReloadScene");
    }
    //Reloads the scene after 1 second
    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Setter getters
    public void SetSliceable(Sliceable slice)
    {
        this._sliceable = slice;
    }
}
