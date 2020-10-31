using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMove : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 currentPos;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        startPos = this.transform.position;
        endPos = this.transform.position + new Vector3(50, 0, 50);
        currentPos = endPos;
        agent.SetDestination(currentPos);
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 1)
        {
            currentPos = currentPos == startPos ? endPos : startPos;
            agent.SetDestination(currentPos);
        }
    }
}
