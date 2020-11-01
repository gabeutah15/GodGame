using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    GameObject currentPickedUpOriginalObject;
    GameObject currentPickedUpPickupableObject;
    Vector3 throwVector = Vector3.zero;
    private Vector3 lastMousePos;
    bool hasReleasedInitialGrabClick = false;
    [HideInInspector]
    public Dictionary<GameObject, GameObject> pickupDictionary = new Dictionary<GameObject, GameObject>();

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
        WorldHand.hasObjectPickedUp = true;
    }

    //if not high enough or enough force applied then should be able to set down and never have to be thrown and wait
    //for stop

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
                WorldHand.hasObjectPickedUp = false;
                currentPickedUpPickupableObject.transform.SetParent(null);
                Rigidbody rb = currentPickedUpPickupableObject.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                throwVector = throwVector.normalized * 30;
                currentPickedUpPickupableObject.GetComponent<Rigidbody>().AddForce(throwVector, ForceMode.Impulse);
                Debug.Log(throwVector);
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
