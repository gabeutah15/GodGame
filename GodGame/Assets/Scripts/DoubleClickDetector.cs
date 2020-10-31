using UnityEngine;
using System.Collections;

public class DoubleClickDetector : MonoBehaviour
{

    private int lmb = 0;
    private int rmb = 0;
    private float timer = 0.0f;
    public float doubleClickTimeWindow = 0.3f;

    public bool WasDoubleClick = false;

    public bool IsDoubleClickLeft()
    {
        bool isDoubleClick = lmb == 2;
        if (isDoubleClick)
        {
            WasDoubleClick = true;
            lmb = 0;
        }


        return isDoubleClick;
    }
    public bool IsDoubleClickRight()
    {
        bool isDoubleClick = rmb == 2;
        if (isDoubleClick)
            lmb = 0;
        return isDoubleClick;
    }

    public void Update()
    {

        timer += Time.deltaTime;

        if (timer > doubleClickTimeWindow)
        {
            lmb = 0;
            rmb = 0;

        }

        if (Input.GetMouseButtonDown(0))
        {
            lmb++;
            timer = 0.0f;
            WasDoubleClick = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            rmb++;
            timer = 0.0f;
        }
    }
}
