using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageAndBuildingManager : MonoBehaviour
{
    Building[] startingBuildings;

    List<Building> allbuildings;//expandable list of buildings
    Village[] allVillages;//can be an array because cannot found new villages, only expand existing


    // Start is called before the first frame update
    void Start()
    {
        allVillages = GameObject.FindObjectsOfType<Village>();//just get component village on these when needed?
        startingBuildings = GameObject.FindObjectsOfType<Building>();
        allbuildings = new List<Building>();//needs to be expandable
        foreach (var building in startingBuildings)
        {
            allbuildings.Add(building);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
