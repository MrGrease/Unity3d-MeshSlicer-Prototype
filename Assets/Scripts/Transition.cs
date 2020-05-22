using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trigger class for the fade to black transition animation
public class Transition : MonoBehaviour
{
    public Animator trans;
    //Starts the transition animation
    public void StartAnim()
    {   //Trigger the transition
        trans.SetTrigger("Start");
    }
}
