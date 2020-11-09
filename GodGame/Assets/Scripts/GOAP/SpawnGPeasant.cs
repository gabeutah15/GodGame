using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGPeasant : MonoBehaviour
{
    public GameObject gPeasantPrefab;
    public int numStartingGPeasants;
    public Transform spawnLocation;
    public Village thisSpawnersVillage;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numStartingGPeasants; i++)
        {
            //GameObject peasant = Instantiate(gPeasantPrefab, spawnLocation.position + new Vector3(i,0,i), Quaternion.identity);
            //peasant.GetComponent<GPeasant>().village = thisSpawnersVillage;//wont work, this is needed on start for peasant
            //instead i'm assigning a village to the GOAPpeasant prefab for testing but ultimately that won't work
            //unless i want a different peasant prefab for each village
        }

        //Jobs and houses and such locations are assigned on start for a peasant only so long as there are sufficient buildings to assign them to
        //will need something like in BW1 where excess peasants are assigned temporary worshipper status and just go worship until they get a task or starve or whatever
        //or maybe make them idle
        Invoke("SpawnAGPeasant", 5);//remember to do this for other things you want done only once a few seconds after 
        //starting
    }

    void SpawnAGPeasant()
    {
        Instantiate(gPeasantPrefab, spawnLocation.position, Quaternion.identity);
        Invoke("SpawnAGPeasant", Random.Range(2, 10));
    }
    // Update is called once per frame
    void Update()
    {

    }
}
