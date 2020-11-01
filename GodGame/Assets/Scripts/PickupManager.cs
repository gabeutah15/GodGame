﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    GameObject currentPickedUpOriginalObject;
    GameObject currentPickedUpPickupableObject;
    Vector3 throwVector = Vector3.zero;
    Vector3 throwVectorTest = Vector3.zero;

    private Vector3 lastMousePos;
    bool hasReleasedInitialGrabClick = false;
    [HideInInspector]
    public Dictionary<GameObject, GameObject> pickupDictionary = new Dictionary<GameObject, GameObject>();
    [SerializeField]
    private float addedYVelocityScalar = 5;
    [SerializeField]
    private float throwStrength = 40;

    //***you can pickup another peasant before teh first one has stopped, which is problem for reincarnating the 
    //original as a navmeshagent

    // Start is called before the first frame update
    void Start()
    {
        lastMousePos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldHand.hasObjectPickedUp)
        {
            DetectHoldAndReleaseMouse();
            if (hasReleasedInitialGrabClick)
            {
                lastMousePos = Input.mousePosition;
            }
        }
    }

    public void PickupNewObject(GameObject original, GameObject pickupAble)
    {
        hasReleasedInitialGrabClick = false;
        pickupDictionary.Add(pickupAble, original);
        currentPickedUpOriginalObject = original;
        currentPickedUpPickupableObject = pickupAble;
        original.SetActive(false);
        pickupAble.transform.SetParent(WorldHand.Hand.transform);
        pickupAble.transform.position = WorldHand.Hand.transform.position;
        WorldHand.hasObjectPickedUp = true;
        WorldHand.heightIncrease = 4.0f;

    }

    //if not high enough or enough force applied then should be able to set down and never have to be thrown and wait
    //for stop

    //currently this will not let you pick up again until the object inquestion has stoped and converted back into navmeshagent, which does not feel right

    public void ThrowableHasHitGroundAndStopped(GameObject throwAble)
    {
        GameObject original = pickupDictionary[throwAble];
        pickupDictionary.Remove(throwAble);
        original.SetActive(true);
        original.transform.position = throwAble.transform.position;
        Destroy(throwAble);

    }

    private void DetectHoldAndReleaseMouse()
    {
        if (hasReleasedInitialGrabClick)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
                throwVector += new Vector3(deltaMousePos.x, 0, deltaMousePos.y) * 0.1f;
            }

            if (Input.GetMouseButtonUp(0))
            {
                throwVector = Camera.main.transform.TransformDirection(throwVector);
                throwVector = new Vector3(throwVector.x, 0, throwVector.z);
                float horizontalMagnitude = throwVector.magnitude;
                throwVector += new Vector3(0, addedYVelocityScalar * horizontalMagnitude, 0);
                throwVector *= throwStrength;

                WorldHand.hasObjectPickedUp = false;
                WorldHand.heightIncrease = 1.0f;

                if (currentPickedUpPickupableObject)//sometimes goes in here again after throwing and then pickupable is null, order of click ups and downs and hasreleased and so on still a little buggy
                {
                    currentPickedUpPickupableObject.transform.SetParent(null);
                    Rigidbody rb = currentPickedUpPickupableObject.GetComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.None;
                    currentPickedUpPickupableObject.GetComponent<Rigidbody>().AddForce(throwVector, ForceMode.Impulse);
                }
                throwVector = Vector3.zero;
                hasReleasedInitialGrabClick = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            hasReleasedInitialGrabClick = true;
        }
    }

    public void DropOrThrowObject()
    {

    }
}
