using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    GameObject currentPickedUpOriginalObject;
    GameObject currentPickedUpObject;
    Vector3 throwVector = Vector3.zero;
    Vector3 throwVectorTest = Vector3.zero;

    private Vector3 lastMousePos;
    bool hasReleasedInitialGrabClick = false;
    [HideInInspector]
    public Dictionary<GameObject, GameObject> pickupNavMeshBodyAndRelatedDictionary = new Dictionary<GameObject, GameObject>();
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
        if (WorldHand.hasObjectPickedUp)//WorldHand.hasObjectPickedUp
        {
            timerAfterThrow = 0;
        }
        else
        {
            timerAfterThrow += Time.deltaTime;
        }

        if((timerAfterThrow > 0.2f) && !WorldHand.handCollider.enabled)
        {
            WorldHand.handCollider.enabled = true;
        }

        //if (WorldHand.hasObjectPickedUp)
        //{
            DetectHoldAndReleaseMouse();
            if (hasReleasedInitialGrabClick)
            {
                lastMousePos = Input.mousePosition;
            }
        //}
    }

    public void PickupNewNavMeshObject(GameObject original, GameObject pickupAble)
    {
        hasReleasedInitialGrabClick = false;
        pickupNavMeshBodyAndRelatedDictionary.Add(pickupAble, original);
        currentPickedUpOriginalObject = original;
        currentPickedUpObject = pickupAble;
        original.SetActive(false);
        pickupAble.transform.SetParent(WorldHand.Hand.transform);
        pickupAble.transform.position = WorldHand.Hand.transform.position;
        WorldHand.hasObjectPickedUp = true;
        //WorldHand.heightIncrease = 4.0f;
        WorldHand.setHeightIncreaseExtendedForPickup();
    }

    //this is for picking up an object associated with a navmeshagent that does not need to be addedd to dictioanry because you just threw it but it never hit the ground and stoppped
    //this is not for throwing non navmesh agent stuff like rocks
    //****this is not called yet
    public void PickupAlreadyThrownNavMeshRelatedBody(GameObject pickupAble)
    {
        hasReleasedInitialGrabClick = false;
        //pickupDictionary.Add(pickupAble, original);
        currentPickedUpOriginalObject = pickupNavMeshBodyAndRelatedDictionary[pickupAble];//original;
        currentPickedUpObject = pickupAble;
        //original.SetActive(false);
        pickupAble.transform.SetParent(WorldHand.Hand.transform);
        pickupAble.transform.position = WorldHand.Hand.transform.position;
        WorldHand.hasObjectPickedUp = true;
        //WorldHand.heightIncrease = 4.0f;
        WorldHand.setHeightIncreaseExtendedForPickup();

    }

    public void PickupPhysicsObject(GameObject pickupAble)
    {
        Rigidbody physicsBodyRb = pickupAble.GetComponent<Rigidbody>();
        physicsBodyRb.useGravity = false;
        physicsBodyRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        //pickupAble.GetComponent<Collider>().enabled = false;
        WorldHand.handCollider.enabled = false;//do i need to do this on picking up the navmesh hand?
        hasReleasedInitialGrabClick = false;
        //pickupNavMeshBodyAndRelatedDictionary.Add(pickupAble, original);
        currentPickedUpObject = pickupAble;
        pickupAble.transform.SetParent(WorldHand.Hand.transform);
        //pickupAble.transform.position = WorldHand.Hand.transform.position;//is setting this causing a feedback loop between the normalized to surface hand position and the parented physics object?
        WorldHand.hasObjectPickedUp = true;
        //WorldHand.heightIncrease = 4.0f;
        WorldHand.setHeightIncreaseExtendedForPickup();

    }

    //if not high enough or enough force applied then should be able to set down and never have to be thrown and wait
    //for stop

    //currently this will not let you pick up again until the object inquestion has stoped and converted back into navmeshagent, which does not feel right

    public void ThrowableVersionOfNavMeshAgentHasHitGroundAndStopped(GameObject throwAble)
    {
        GameObject original = pickupNavMeshBodyAndRelatedDictionary[throwAble];
        pickupNavMeshBodyAndRelatedDictionary.Remove(throwAble);
        original.SetActive(true);
        original.transform.position = throwAble.transform.position;
        Destroy(throwAble);//this is a former currentPickedUpObject
        //Debug.Log("hit ground and stopped");
    }

    private float timerAfterThrow = 0f;

    private void DetectHoldAndReleaseMouse()
    {
        if (!currentPickedUpObject)
        {
            hasReleasedInitialGrabClick = false;
            WorldHand.hasObjectPickedUp = false;
            //WorldHand.heightIncrease = 1.0f;
            WorldHand.setHeightIncreaseNormal();

        }

        if (hasReleasedInitialGrabClick)
        {

            if (Input.GetMouseButton(0))
            {
                Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
                throwVector += new Vector3(deltaMousePos.x, 0, deltaMousePos.y) * 0.1f;
            }

            if (Input.GetMouseButtonUp(0))
            {
                currentPickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                throwVector = Camera.main.transform.TransformDirection(throwVector);
                throwVector = new Vector3(throwVector.x, 0, throwVector.z);
                float horizontalMagnitude = throwVector.magnitude;
                throwVector += new Vector3(0, addedYVelocityScalar * horizontalMagnitude, 0);
                throwVector *= throwStrength;

                WorldHand.hasObjectPickedUp = false;
                //WorldHand.heightIncrease = 1.0f;
                WorldHand.setHeightIncreaseNormal();


                if (currentPickedUpObject)//sometimes goes in here again after throwing and then pickupable is null, order of click ups and downs and hasreleased and so on still a little buggy
                {
                    currentPickedUpObject.transform.SetParent(null);
                    Rigidbody rb = currentPickedUpObject.GetComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.None;
                    currentPickedUpObject.GetComponent<Rigidbody>().AddForce(throwVector, ForceMode.Impulse);
                }
                throwVector = Vector3.zero;
                hasReleasedInitialGrabClick = false;
                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {


            if (!hasReleasedInitialGrabClick)
            {
               // timerAfterInitialReleaseAfterPickup += Time.deltaTime;
                //if(timerAfterInitialReleaseAfterPickup > 0.3f)
               // {
                    //start timer, once timer high enough set to true and then set timer back to 0
                    hasReleasedInitialGrabClick = true;
                   // timerAfterInitialReleaseAfterPickup = 0f;
                //}
            }
        }
    }

    public void DropOrThrowObject()
    {

    }
}
