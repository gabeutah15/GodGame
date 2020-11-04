using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Village village;
    public bool isWorkBuilding;
    public Jobs associatedJob;
    public int maxNumWorkers;
    public int minNumWorkers;
    public int currentNumWorkers;
    WorkWaypoints workWayPoints;
    public List<Peasant> workers;
    public ArrivalPoint arrivalPoint;


    // Start is called before the first frame update
    void Awake()
    {
        workers = new List<Peasant>();//will be empty for non work buildings, better to have a workBuildings class derived from building? i dunno, there won't be that many buildings, maybe isWorkBuilding bool is enough
        arrivalPoint = GetComponentInChildren<ArrivalPoint>();

        village.buildingsForThisVillage.Add(this);//dunno if this sort of thing is a code smell, also wil have to expand village lists during gameplay as buildings are made and destroyed
        if (isWorkBuilding)
        {
            workWayPoints = GetComponentInChildren<WorkWaypoints>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessWorkerOnArrival(GameObject peasant)
    {
        if (isWorkBuilding)
        {
            workWayPoints.AddPeasantNavAgentToWayPointPath(peasant.GetComponent<Peasant>());//should maybe just be passing this around as a peasant everywhere?
        }
    }

    public void OutProcessWorkerOnLeaving(GameObject peasant)//these are just 
    {
        if (isWorkBuilding)
        {
            workWayPoints.RemovePeasantNavAgentFromWayPointPath(peasant.GetComponent<Peasant>());//should maybe just be passing this around as a peasant everywhere?
            //they will also need to have new destinations set though? that can be elsewhere, where the time of day changes, tghis just releives them of their waypooints
        }
    }
}
