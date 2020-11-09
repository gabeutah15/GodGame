using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

//public enum Ages
//{
//    Child,
//    Adult,
//    Old
//}

//public enum Jobs
//{
//    Unemployed,
//    Farmer,
//    PigFarmer,
//    Shepherd,
//    //Worshipper
//}

//public enum TaskTime
//{
//    Sleep,
//    Work,
//    Play
//}

public class Peasant : MonoBehaviour
{
    //set by RandomPeasantStatsGenerator:
    public string Name { get; set; }
    public int Health { get; set; }
    public bool IsMale { get; set; }
    public Ages Age { get; set; }
    public float WalkingSpeed { get; set; }//just to have some variation in speeds?
    public float RunningSpeed { get; set; }
    //end RandomPeasantStatsGenerator stats

    public Jobs Job { get; set; }//should i also have a job building parameter? or based on job just get assigned a random nearby building of the right type whenever need to go work? the latter might be 
    //easier because would actively account for buildings being destroyed
    RandomPeasantStatsGenerator statsGenerator;
    TextMeshProUGUI statsDisplay;
    public string displayText;
    NavMeshAgent agent;
    public Village village;
    MeshRenderer meshRend;
    public TaskTime taskTime;
    Building workBuilding;
    Building leisureBuilding;//only one of these per peasant? other leisure activities? only one of these per village?
    Building homeBuilding;

    private float dayTimer = 0f;

    //a lot of this will have to be moved to awake once spawning new peasants?
    private void Start()
    {
        statsGenerator = FindObjectOfType<RandomPeasantStatsGenerator>();
        PeasantStats generatedStats = statsGenerator.GenerateNewPeasantStats();
        Health = generatedStats.health;
        IsMale = generatedStats.isMale;
        Age = generatedStats.age;
        WalkingSpeed = generatedStats.walkingSpeed;
        RunningSpeed = generatedStats.runningSpeed;
        Name = generatedStats.name;
        Job = generatedStats.job;
        statsDisplay = GetComponentInChildren<TextMeshProUGUI>();
        //assign everything on awake because people will be spawned and killed nad buildings made and destroyed throughout the game
        //need some master list of all player controlled buildings that expands and contracts
        SetDisplayText();//recall this whenever age or job changes
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = WalkingSpeed;
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = village.villageColor;
        village.villagers.Add(this);

        foreach (Building building in village.buildingsForThisVillage)
        {
            if (building.isWorkBuilding)
            {
                if (!workBuilding)
                {
                    if (building.associatedJob == Job)//this will only work
                    {
                        if (building.numWorkersAssignedToThisLocation < building.maxNumWorkersAssignedToThisLocation)
                        {
                            workBuilding = building;
                            //building.workers.Add(this);//building will add worker when they 
                            //arrive? no that is for waypoints because needed only when acrively working, 
                            //building can track it's workers all teh time


                            break;
                        }
                    }
                }
            }
            else if (building.GetComponent<House>())
            {
                if (!homeBuilding)
                {
                    House house = building.GetComponent<House>();
                    if (house.GetNumOccupants() < house.maxOccupants)
                    {
                        //add an occupant to the house until it is full
                        //house.AddOccupant(this);//removed because using GPeasantNow
                        homeBuilding = building;
                    }
                }
            }
            else if (building.GetComponent<LeisureRoute>())
            {
                leisureBuilding = building;
            }
        }
        if (workBuilding)
        {
            GoToWork(agent);
        }
        else
        {
            //everyone should at least have leisure building
            GoToLeisure(this);//unneccessary method if just passes this though
        }
    }

    private void Awake()//awake is before start
    {
        taskTime = (TaskTime)Random.Range(0, 3);//arbitrary 3 because 3 task times currentluy, will need to change later
                                                //this sort of thing should probably be in like a time manager, not on each peasant, except it might be good to have them
                                                //all on their own time schedules? or maybe a time manager with some randomization
                                                //by here the villages building list should be populated

    }

    public void GoToWork(NavMeshAgent p_agent)
    {
        workBuilding.arrivalPoint.AddAgentOnTheWay(p_agent);
    }

    public void GoToLeisure(Peasant peasant)
    {
        leisureBuilding.workWayPoints.AddPeasantNavAgentToWayPointPath(peasant);
    }

    public void GoHome(NavMeshAgent p_agent)
    {
        homeBuilding.arrivalPoint.AddAgentOnTheWay(agent);
    }

    private void SetDisplayText()
    {
        displayText = Name + '\n' + (IsMale ? "Male" : "Female") + '\n' + Age + '\n' + Job;
        statsDisplay.text = displayText;
    }

    //bool goneToWork = false;

    // Update is called once per frame
    void Update()
    {
        //if (!goneToWork && dayTimer > 1)
        //{
        //    //agent.SetDestination(this.transform.position + new Vector3(10,0,10));//this works, so the agent definitely exists
        //    //GoToWork(/*this.gameObject.GetComponent<Peasant>()*/);
        //    goneToWork = true;
        //}

        //this whole section is just a test schedule
        dayTimer += Time.deltaTime;
        if (dayTimer > 15)
        {
            int taskTimeIndex = (int)taskTime;
            taskTimeIndex++;
            taskTimeIndex = taskTimeIndex % 3;
            taskTime = (TaskTime)taskTimeIndex;
            dayTimer = 0;
            BreakAllBuildingAttachments();

            if (taskTime == TaskTime.Work)
            {
                if (workBuilding)
                {
                    GoToWork(agent);
                }
                else
                {
                    GoToLeisure(this);
                }
            }
            else if (taskTime == TaskTime.Play)
            {
                GoToLeisure(this);
            }
            else//sleep
            {
                GoHome(agent);
            }
        }
    }

    public void BreakAllBuildingAttachments()
    {
        //this is just clearing everything, but will it cause errors if cleared from something not in or from building no longer exists?
        //house and work have arrival point, but leisure does not
        if (workBuilding)
        {
            workBuilding.arrivalPoint.RemoveAgentOnTheWay(agent);//what happens if call this and not present in list
            //workBuilding.workWayPoints.RemovePeasantNavAgentFromWayPointPath(this);//below calls this
            workBuilding.OutProcessWorkerOnLeaving(this.gameObject);
        }

        if (homeBuilding)
        {
            homeBuilding.arrivalPoint.RemoveAgentOnTheWay(agent);
            homeBuilding.OutProcessWorkerOnLeaving(this.gameObject);//this makes unique call for home, just turns visual and collider back on
        }
        if (leisureBuilding)
        {
            //leisureBuilding.workWayPoints.RemovePeasantNavAgentFromWayPointPath(this);//below calls this
            leisureBuilding.OutProcessWorkerOnLeaving(this.gameObject);
        }
        //everyone should have a home and leisure building, how about don't limit num occupants but just make them less happy the more cramped they are
        //work and leisure have workwaypoints but house does not

    }

    private void AgeUp()
    {
        //should so some age class as a property so you can just say like age = age+1 rather than having to re assign a bunch of health and speed stats and stuff
        if (Age == Ages.Old)
        {
            Die();
        }
        else if (Age == Ages.Adult)
        {
            Health = HealthValues.OldHealth;
            RunningSpeed = LocomotionSpeeds.OldRun;
            WalkingSpeed = LocomotionSpeeds.OldWalk;
            Age = Ages.Old;
        }
        else if (Age == Ages.Child)
        {
            Health = HealthValues.AdultHealth;
            RunningSpeed = LocomotionSpeeds.AdultRun;
            WalkingSpeed = LocomotionSpeeds.AdultWalk;
            Age = Ages.Adult;
        }
    }

    private void Die()
    {

    }
}
