using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArrivalPoint : MonoBehaviour
{
    Vector3 arrivalPoint;
    List<NavMeshAgent> agentsComingToWorkHere;
    Building associatedBuilding;
    // Start is called before the first frame update
    void Awake()//awake is before start, need to initialize these so can assign workers on game start
    {
        agentsComingToWorkHere = new List<NavMeshAgent>();
        associatedBuilding = GetComponentInParent<Building>();

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
        //int numAgents = agentsComingToWorkHere.Count;
        //for (int i = 0; i < numAgents; i++)
        //{
        //    NavMeshAgent agent = agentsComingToWorkHere[i];
        //    if (agent.remainingDistance < 2)
        //    {
        //        associatedBuilding.ProcessWorkerOnArrival(agent.gameObject);
        //        RemoveAgentOnTheWay(agent/*.gameObject.GetComponent<Peasant>()*/);//need to pass these around just as peasants I think?
        //    }
        //}

        
        for (int i = agentsComingToWorkHere.Count - 1; i >= 0; i--)
        {
            NavMeshAgent agent = agentsComingToWorkHere[i];
            if (agent.remainingDistance < 2)
            {
                associatedBuilding.ProcessWorkerOnArrival(agent.gameObject);
                RemoveAgentOnTheWay(i/*agent*//*.gameObject.GetComponent<Peasant>()*/);//need to pass these around just as peasants I think?
            }


            //if (list[i] > 5)
            //    list.RemoveAt(i);
        }
        //list.ForEach(i => Console.WriteLine(i));

        //foreach (NavMeshAgent agent in agentsComingToWorkHere)
        //{
        //    if(agent.remainingDistance < 2)
        //    {
        //        associatedBuilding.ProcessWorkerOnArrival(agent.gameObject);
        //        RemoveAgentOnTheWay(agent/*.gameObject.GetComponent<Peasant>()*/);//need to pass these around just as peasants I think?
        //    }
        //}
    }

    public void AddAgentOnTheWay(/*Peasant peasant*/NavMeshAgent agent)
    {
        //NavMeshAgent agent = peasant.GetComponent<NavMeshAgent>();
        agentsComingToWorkHere.Add(agent);
        agent.SetDestination(arrivalPoint);
    }

    public void RemoveAgentOnTheWay(/*Peasant peasant*//*NavMeshAgent agent*/int i)
    {
        agentsComingToWorkHere.RemoveAt(/*peasant.GetComponent<NavMeshAgent>()*//*agent*/i);//might make an overloadf that also uses remove for one offs not in a for loop?
    }
}
