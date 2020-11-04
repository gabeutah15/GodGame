using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum Ages
{
    Child,
    Adult,
    Old
}

public enum Jobs
{
    Unemployed,
    Farmer,
    PigFarmer,
    Shepherd,
    Worshipper
}

public enum TaskTime
{
    Sleep,
    Work,
    Play
}

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

    public House House { get; set; }
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
        village.Villagers.Add(this);

        foreach (Building building in village.buildingsForThisVillage)
        {
            if (building.isWorkBuilding)
            {
                if (building.associatedJob == Job)
                {
                    if (building.currentNumWorkers < building.maxNumWorkers)
                    {
                        workBuilding = building;
                        building.workers.Add(this);//building will add worker when they arrive? no that is for waypoints because needed only when acrively working, building can track it's workers all teh time
                        break;
                    }
                }
            }
        }
        if (workBuilding)
        {
            GoToWork(agent);
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

        dayTimer += Time.deltaTime;
        if(dayTimer > 60)
        {
            int taskTimeIndex = (int)taskTime;
            taskTimeIndex++;
            taskTimeIndex = taskTimeIndex % 3;
            taskTime = (TaskTime)taskTimeIndex;
            dayTimer = 0;
        }
    }

    private void AgeUp()
    {
        //should so some age class as a property so you can just say like age = age+1 rather than having to re assign a bunch of health and speed stats and stuff
        if(Age == Ages.Old)
        {
            Die();
        }
        else if(Age == Ages.Adult)
        {
            Health = HealthValues.OldHealth;
            RunningSpeed = LocomotionSpeeds.OldRun;
            WalkingSpeed = LocomotionSpeeds.OldWalk;
            Age = Ages.Old;
        }
        else if(Age == Ages.Child)
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
