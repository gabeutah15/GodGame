using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    Rigidbody rb;
    PickupManager pickupManager;
    private bool hasHitGround = false;

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
        if (hasHitGround)
        {
            if(rb.velocity.sqrMagnitude < .01)//maybe change to less than epsilon or something later
            {
                pickupManager.ThrowableHasHitGroundAndStopped(this.gameObject);
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
