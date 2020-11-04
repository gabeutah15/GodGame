using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    //track resoruces on the village level
    //track all buildings associated with this village (will need to be expandable list as buildings are added
    List<Building> buildingsForThisVillage;//village has list of all of its buildings and each building has a reference to it's village, probably ok? mutual reference bad?
    List<Peasant> Villagers;
    public Color villageColor;

    //spawn more vilalgers from here so you can add them to the list
    //populate list initially with outstanding villagers, and then expand when spawning

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
