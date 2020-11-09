using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class Building : MonoBehaviour
{
    //**this has too much working building specific stuff, should probably put that in a child class***
    public Village village;
    public bool isWorkBuilding;
    public bool isLeisureBuilding;

    //public bool isHouse;
    public Jobs associatedJob;
    public int maxNumWorkersAssignedToThisLocation;
    //public int maxNumWorkersAtOnce;
    public int minNumWorkersToFunction;
    [HideInInspector]
    public int numWorkersAssignedToThisLocation;
    [HideInInspector]
    public WorkWaypoints workWayPoints;
    //public List<Peasant> workers;
    public List<GPeasant> workers;

    [HideInInspector]
    public ArrivalPoint arrivalPoint;
    private bool isHouse;


    // Start is called before the first frame update
    void Awake()
    {
        workers = new List<GPeasant>();//will be empty for non work buildings, better to have a workBuildings class derived from building? i dunno, there won't be that many buildings, maybe isWorkBuilding bool is enough
        arrivalPoint = GetComponentInChildren<ArrivalPoint>();
        workWayPoints = GetComponentInChildren<WorkWaypoints>();
        

        village.buildingsForThisVillage.Add(this);//dunno if this sort of thing is a code smell, also wil have to expand village lists during gameplay as buildings are made and destroyed
        if (isWorkBuilding)
        {
            workWayPoints = GetComponentInChildren<WorkWaypoints>();
        }
        isHouse = false;
        if (this.GetComponent<House>())
        {
            isHouse = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float originalSpeed = 0;

    public void ProcessWorkerOnArrival(GameObject peasant)
    {
        if (isWorkBuilding)
        {
            workWayPoints.AddPeasantNavAgentToWayPointPath(peasant.GetComponent<Peasant>());//should maybe just be passing this around as a peasant everywhere?
        }
        else if(isHouse)
        {
            //this should mean it's a leisure rout, because houses do not call proces
            //no work waypoints they just go inside/disappear
            
            peasant.GetComponent<MeshRenderer>().enabled = false;//SetActive(false)//this doens't work because then cannot access again, it's update stops calling, but perhaps if this is handled by
                                                                 //a manager and not the peasants own script later that won't be a problem;//ie like they went inside
            peasant.GetComponent<Collider>().enabled = false;
            //originalSpeed = peasant.GetComponent<NavMeshAgent>().speed;//**this is setting everyone's speeds to zero all at once? indicates problem with when this is called
            //peasant.GetComponent<NavMeshAgent>().speed = 0;
        }
    }

    public void OutProcessWorkerOnLeaving(GameObject peasant)//these are just 
    {
        if (isWorkBuilding || isLeisureBuilding)
        {

            workWayPoints.RemovePeasantNavAgentFromWayPointPath(peasant.GetComponent<Peasant>());//should maybe just be passing this around as a peasant everywhere?
            //they will also need to have new destinations set though? that can be elsewhere, where the time of day changes, tghis just releives them of their waypooints
            //arrivalPoint.RemoveAgentOnTheWay(peasant.GetComponent<NavMeshAgent>());//don't do this here because if in building then not on arrival point
        }
        else if (isHouse)
        {
            //Don't do below if this is result of being picked up and put down
            peasant.GetComponent<MeshRenderer>().enabled = true;
            peasant.GetComponent<Collider>().enabled = true;
            //peasant.GetComponent<NavMeshAgent>().speed = originalSpeed;//***this hacky way of doing this, instead find out why being given more waypoints after reaching house
        }
        else
        {
            int a = 5;//test, should not go here
        }
    }
}
