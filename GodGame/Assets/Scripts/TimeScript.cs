using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    [HideInInspector]
    public static bool IsPausedTurn = true;
    //related scripts:
    //turnoffbutton
    //turnofftext
    //turnofftextreverse
    //also underUI Canvas Pause Text, Play Button, and Real Time Text

    // Use this for initialization
    void Start()
    {
        //Time.timeScale = 0f;//uncomment if restoring pausing
        IsPausedTurn = true;
        //InvokeRepeating("AutoPause", 0f, 10.0f);//uncomment if you want to bring pausing back

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("The Game Is Paused? " + IsPausedTurn);
    }

    public void DoubleTime()
    {
        IsPausedTurn = false;
        Time.timeScale = 2.0f;
    }

    public void NormalTime()
    {
        IsPausedTurn = false;
        Time.timeScale = 1.0f;
    }

    public void PauseTime()
    {
        IsPausedTurn = true;
        Time.timeScale = .01f;
    }

    void AutoPause()
    {
        Time.timeScale = .01f;
        IsPausedTurn = true;
    }
}
