using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkWaypoints : MonoBehaviour
{
    [SerializeField]
    Transform[] wayPoints;
    //int destinationIndex;
    Dictionary<NavMeshAgent, int> peasantNavMeshAgentsCurrentlyWorkingThisField;//will be drawn from villagers assigned this job at this location? or villager just added when they arrive to parent location?

    void Start()
    {
        peasantNavMeshAgentsCurrentlyWorkingThisField = new Dictionary<NavMeshAgent, int>();

    }

    private void Awake()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            Vector3 arrivalPoint = wayPoints[i].position;
            NavMeshHit myNavHit;
            if (NavMesh.SamplePosition(arrivalPoint, out myNavHit, 10, -1))//does this use like a sphere to find the nearest point?
            {
                wayPoints[i].position = myNavHit.position;//so to nearest point on navmesh
            }
        }

    }

    void Update()
    {
        List<NavMeshAgent> keys = new List<NavMeshAgent>(peasantNavMeshAgentsCurrentlyWorkingThisField.Keys);//this is probably not performant?
        foreach (NavMeshAgent agent in keys)
        {
            if (agent.remainingDistance < 1)
            {
                int destinationIndex = peasantNavMeshAgentsCurrentlyWorkingThisField[agent];//i can alter the values if i am iterating through the keys?
                destinationIndex++;
                destinationIndex = destinationIndex % wayPoints.Length;
                peasantNavMeshAgentsCurrentlyWorkingThisField[agent] = destinationIndex;
                GoToDestination(agent, destinationIndex);
            }
        }

        //foreach (NavMeshAgent agent in peasantNavMeshAgentsCurrentlyWorkingThisField.Keys)
        //{
        //    if (agent.remainingDistance < 1)
        //    {
        //        int destinationIndex = peasantNavMeshAgentsCurrentlyWorkingThisField[agent];//i can alter the values if i am iterating through the keys?
        //        destinationIndex++;
        //        destinationIndex = destinationIndex % wayPoints.Length;
        //        peasantNavMeshAgentsCurrentlyWorkingThisField[agent] = destinationIndex;
        //        GoToDestination(agent, destinationIndex);
        //    }
        //}

    }

    public void GoToDestination(NavMeshAgent agent, int index)
    {
        agent.SetDestination(wayPoints[index].position);
    }

    public void AddPeasantNavAgentToWayPointPath(Peasant peasant)
    {
        //for some reason cannot user manual getter and setter or auto property to get navmeshagent from peasant?
        peasantNavMeshAgentsCurrentlyWorkingThisField.Add(peasant.GetComponent<NavMeshAgent>(), Random.Range(0, wayPoints.Length));//start at random waypoint
    }

    public void RemovePeasantNavAgentFromWayPointPath(Peasant peasant)
    {
        peasantNavMeshAgentsCurrentlyWorkingThisField.Remove(peasant.GetComponent<NavMeshAgent>());//can remove with just key i think?
    }
}
