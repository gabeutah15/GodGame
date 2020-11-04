using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PeasantNavigation : MonoBehaviour
{
    Vector3 currentTarget;
    public Building Home { get; set; }
    public Building PlaceOfWork { get; set; }
    NavMeshAgent agent;
    //this class might be unneccessary if all it calls is set destination

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewTargetDestination(Vector3 target)
    {
        currentTarget = target;
    }
}
