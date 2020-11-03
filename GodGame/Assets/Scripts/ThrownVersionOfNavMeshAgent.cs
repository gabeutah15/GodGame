using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownVersionOfNavMeshAgent : MonoBehaviour
{
    Rigidbody rb;
    PickupManager pickupManager;
    private bool hasHitGround = false;
    private float timer;
    private float timeUntilCanHitGroundAgain = 0.2f;//added so not sometimes lose pickup as soon as pickup cuz not moving fast and still on ground

    private void Awake()
    {
        this.transform.SetParent(WorldHand.Hand.transform);
        rb = this.GetComponent<Rigidbody>();
        pickupManager = FindObjectOfType<PickupManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (hasHitGround && timer >= timeUntilCanHitGroundAgain)
        {
            if(rb.velocity.sqrMagnitude < .2)//maybe change to less than epsilon or something later
            {
                pickupManager.ThrowableVersionOfNavMeshAgentHasHitGroundAndStopped(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasHitGround = true;
        }
    }


}
