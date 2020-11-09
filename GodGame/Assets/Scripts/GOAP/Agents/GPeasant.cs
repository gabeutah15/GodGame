using System;
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
    Unemployed,//nothing for unemployed to do yet
    Farmer,
    PigFarmer,
    Shepherd,
    //Worshipper//no worshipper building yet
}

public enum TaskTime
{
    Sleep,
    Work,
    Play
}

public class GPeasant : GAgent
{
    //added from peasant:
    //set by RandomPeasantStatsGenerator:
    public string Name { get; set; }
    public int Health { get; set; }
    public bool IsMale { get; set; }
    public Ages Age { get; set; }
    public float WalkingSpeed { get; set; }
    public float RunningSpeed { get; set; }
    //end RandomPeasantStatsGenerator stats

    //all actions
    //ideally there's a better way of doing this, but get all gaction components and then splitting them out
    //based on derived type just doesn't work
    GoToVillageCenter gotToVillageCenter_gAction;
    GoToJob gotToJob_gAction;
    GoToHouse gotToHouse_gAction;


    //end all actions

    public Jobs Job { get; set; }
    RandomPeasantStatsGenerator statsGenerator;
    TextMeshProUGUI statsDisplay;
    public string displayText;
    NavMeshAgent agent;//gotten from GAction, but need to set speed here
    //(is that good, for each action to get the agent component separately?) seems ok i guess
    MeshRenderer meshRend;
    public TaskTime taskTime;
    Building workBuilding;
    Building leisureBuilding;//only one of these per peasant? other leisure activities? only one of these per village?
    Building homeBuilding;

    private float dayTimer = 0f;
    //end added from peasant
    [SerializeField]
    private string villageTag;
    public Village village;
    [HideInInspector]
    public VillageCenter villageCenter;
    private GAction[] localGActionArray;
    // Start is called before the first frame update
    void Start()
    {
        if (!village)//if a spawned peasant, need to find it's village
        {
            var villagesGameObject = GameObject.FindObjectsOfType(typeof(Village));//this is also an empty village?
            //there's never in buildings in below village:
            village = GameObject.FindGameObjectWithTag(villageTag).GetComponent<Village>();
        }

        if (!village)
        {
            Debug.LogError("attempted to spawn g peasant without village");
        }

        base.Start();
        SubGoal s1 = new SubGoal("isHome", 1, true);
        goals.Add(s1, 3);
        villageCenter = village.GetComponentInChildren<VillageCenter>();
        localGActionArray = GetComponents<GAction>();



        //added from peasant
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
        //SetDisplayText();//re-call this whenever age or job changes
        agent = this.GetComponent<NavMeshAgent>();
        agent.speed = WalkingSpeed;
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = village.villageColor;
        //village.Villagers.Add(this);//later change the village.Villagers list to had GPeasant and Not Peasant types

        #region assign worker to building
        foreach (Building building in village.buildingsForThisVillage)//there are no buildings in the village 
                                                                      //for the spawned peasants but there are for the initial peasant
        {
            if (building.isWorkBuilding)
            {
                if (!workBuilding)
                {
                    //if (building.associatedJob == Job)//this will only work
                    //{

                    if (building.numWorkersAssignedToThisLocation < building.maxNumWorkersAssignedToThisLocation)
                    {
                        Job = building.associatedJob;//doing this instead of above filter, so still getting default
                        //job from spawning but will be assigned whatever job is 
                        //needed on spawning
                        workBuilding = building;
                        building.workers.Add(this);//buil, building can track it's workers all teh time
                        building.numWorkersAssignedToThisLocation++;//
                        break;
                    }
                    //}
                }
            }
            else if (building.GetComponent<House>())
            {
                if (!homeBuilding)
                {
                    House house = building.GetComponent<House>();
                    //if (house.GetNumOccupants() < house.maxOccupants)//might introduce this again later
                    //but for now for avoiding errors just assigning as mnay occupants to a house as you need
                    //{
                        //add an occupant to the house until it is full
                        house.AddOccupant(this);
                        homeBuilding = building;
                    //}
                }
            }
            else if (building.GetComponent<LeisureRoute>())
            {
                leisureBuilding = building;
            }
        }
        //end added from peasant
        #endregion

        //adding gActions
        //need better way of doing this, but hard to iterate through all actions and find derived gAction type like GoToJob
        //should probably not filter by max workers on assiging buildgins? no still hage to so everywere gets someone
        //perhaps should not assign the job randomly, but instead based on job availability
        gotToVillageCenter_gAction = this.GetComponent<GoToVillageCenter>();
        if (gotToVillageCenter_gAction && village)
        {
            gotToVillageCenter_gAction.target = village.GetComponentInChildren<VillageCenter>().GetComponentInChildren<ArrivalPoint>().gameObject;
        }
        gotToJob_gAction = this.GetComponent<GoToJob>();
        if (gotToJob_gAction && workBuilding)
        {
            gotToJob_gAction.target = workBuilding.GetComponentInChildren<ArrivalPoint>().gameObject;
        }
        gotToHouse_gAction = this.GetComponent<GoToHouse>();
        if (gotToHouse_gAction && homeBuilding)
        {
            gotToHouse_gAction.target = homeBuilding.GetComponentInChildren<ArrivalPoint>().gameObject;
        }
        //end adding gActions

        //this doesnt work, just always goes to first one, will jsut need to manually have variable for each action
        //
        //foreach (GAction gAction in localGActionArray)
        //{
        //    //probably shouldnt do below this way, this kind of casting is like a code smell or whatever
        //    //but it is nice to only have to drag the assigned village in in inspector, or assign it elsewhere
        //    //and then on start for a given peasant everything else can be assigned
        //    if (typeof(GAction).IsAssignableFrom(typeof(GoToVillageCenter)))
        //    {
        //        Debug.Log("has go to village center action");
        //        gAction.target = village.GetComponentInChildren<VillageCenter>().GetComponentInChildren<ArrivalPoint>().gameObject;
        //    }
        //    else if (typeof(GAction).IsAssignableFrom(typeof(GoToJob)))
        //    {
        //        Debug.Log("has go to job action");
        //        gAction.target = workBuilding.GetComponentInChildren<ArrivalPoint>().gameObject;
        //    }
        //}
        SetDisplayText();//new job assigned after iterating through buildings


    }
    private void SetDisplayText()
    {
        displayText = Name + '\n' + (IsMale ? "Male" : "Female") + '\n' + Age + '\n' + Job;
        statsDisplay.text = displayText;
    }

}
