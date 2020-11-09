using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkWaypoints : MonoBehaviour
{
    [SerializeField]
    Transform[] wayPoints;
    //int destinationIndex;
    Dictionary<NavMeshAgent, int> peasantNavMeshAgentsCurrentlyWorkingThisJob;//will be drawn from villagers assigned this job at this location? or villager just added when they arrive to parent location?
    public float workSpeed = 1f;

    void Start()
    {

    }

    private void Awake()
    {
        peasantNavMeshAgentsCurrentlyWorkingThisJob = new Dictionary<NavMeshAgent, int>();
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

    private float destinationWaitTimer = 0f;
    private float destinationWaitTime = 3f;
    private bool reachedDestination = false;

    void Update()
    {
        //rather than doing all of this, why not just have one farming or whatever animation that plays while at the work station, that can move them around?

        List<NavMeshAgent> keys = new List<NavMeshAgent>(peasantNavMeshAgentsCurrentlyWorkingThisJob.Keys);//this is probably not performant?
        foreach (NavMeshAgent agent in keys)
        {
            if (agent.isActiveAndEnabled)
            {
                if (agent.remainingDistance < 1f)
                {
                    int destinationIndex = peasantNavMeshAgentsCurrentlyWorkingThisJob[agent];//i can alter the values if i am iterating through the keys?
                    destinationIndex++;
                    destinationIndex = destinationIndex % wayPoints.Length;
                    peasantNavMeshAgentsCurrentlyWorkingThisJob[agent] = destinationIndex;
                    reachedDestination = true;
                    GoToDestination(agent, destinationIndex);
                }
            }
        }

        //if (reachedDestination)
        //{
        //    destinationWaitTimer += Time.deltaTime;
        //    if(destinationWaitTimer > destinationWaitTime)
        //    {

        //    }
        //}

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
        //peasantNavMeshAgentsCurrentlyWorkingThisJob is null one first starting sometimes
        peasantNavMeshAgentsCurrentlyWorkingThisJob.Add(peasant.GetComponent<NavMeshAgent>(), Random.Range(0, wayPoints.Length));//start at random waypoint
        peasant.GetComponent<NavMeshAgent>().speed = workSpeed;
    }

    public void RemovePeasantNavAgentFromWayPointPath(Peasant peasant)
    {
        peasantNavMeshAgentsCurrentlyWorkingThisJob.Remove(peasant.GetComponent<NavMeshAgent>());//can remove with just key i think?
        peasant.GetComponent<NavMeshAgent>().speed = peasant.WalkingSpeed;
    }
}
