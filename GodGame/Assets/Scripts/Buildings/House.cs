using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int maxOccupants;
    private List<Peasant> occupants;
    // Start is called before the first frame update
    void Awake()
    {
        occupants = new List<Peasant>();
    }

    public int GetNumOccupants()
    {
        return occupants.Count;
    }

    public void AddOccupant(Peasant occupant)
    {
        occupants.Add(occupant);
        
    }

    public void RemoveOccupant(Peasant occupant)
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
