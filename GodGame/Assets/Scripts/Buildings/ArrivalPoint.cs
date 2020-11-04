using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArrivalPoint : MonoBehaviour
{
    Vector3 arrivalPoint;
    // Start is called before the first frame update
    void Start()
    {
        arrivalPoint = this.transform.position;
        NavMeshHit myNavHit;
        if (NavMesh.SamplePosition(arrivalPoint, out myNavHit, 10, -1))
        {
            arrivalPoint = myNavHit.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
