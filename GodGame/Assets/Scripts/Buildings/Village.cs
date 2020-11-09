using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    //track resoruces on the village level
    //track all buildings associated with this village (will need to be expandable list as buildings are added
    public List<Building> buildingsForThisVillage;//village has list of all of its buildings and each building has a reference to it's village, probably ok? mutual reference bad?
    public List<Peasant> villagers;
    public Color villageColor;

    //spawn more vilalgers from here so you can add them to the list
    //populate list initially with outstanding villagers, and then expand when spawning

    // Start is called before the first frame update
    void Start()
    {
        villagers = new List<Peasant>();
        //buildingsForThisVillage = new List<Building>();//this empties out the list which was populated by the building classes
    }

    // Update is called once per frame
    void Update()
    {
        int test = 5;
    }
}
