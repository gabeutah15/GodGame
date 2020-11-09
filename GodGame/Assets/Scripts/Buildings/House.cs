using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int maxOccupants;
    private List<GPeasant> occupants;//from Peasant
    // Start is called before the first frame update
    void Awake()
    {
        occupants = new List<GPeasant>();
    }

    public int GetNumOccupants()
    {
        return occupants.Count;
    }

    public void AddOccupant(GPeasant occupant)//from Peasand
    {
        occupants.Add(occupant);
        
    }

    public void RemoveOccupant(GPeasant occupant)
    {
        occupants.Remove(occupant);
    }

    public void RemoveOccupantAtIndex(int occupantIndex)
    {
        occupants.RemoveAt(occupantIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
