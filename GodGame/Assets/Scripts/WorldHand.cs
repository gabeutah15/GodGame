using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHand : MonoBehaviour
{
    float zAxis = 2f;
    Vector3 mousePosition;
    [SerializeField]
    LayerMask layerMaskGround;
    Vector3 mousePosOnTerrain;
    [SerializeField]
    GameObject handPrefab;
    public static GameObject Hand;
    Vector3 normalToSurfaceIncrease = new Vector3(0,0,0);
    Vector3 positionOfLastRightCick = Vector3.zero;
    [SerializeField]
    float lerpSpeed = 0.5f;
    public static float heightIncrease = .5f;
    [HideInInspector]
    public static bool hasObjectPickedUp;//dunno about just doing global variables for all hand stuff? maybe ok
    //because there is only one hand? what about multiplayer or vs other gods?
    public static Collider handCollider;

    // Start is called before the first frame update
    void Start()
    {
        Hand = Instantiate(handPrefab);
        handCollider = Hand.GetComponent<Collider>();
    }

    public static void setHeightIncreaseNormal()
    {
        heightIncrease = 0.5f;
    }

    public static void setHeightIncreaseExtendedForPickup()
    {
        heightIncrease = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        //bool hasFinishedDraggingButNotGivenNewCommand;
        //below should not be in update, just set it once when you pick up stuff
        //if (hasObjectPickedUp)
        //{
        //    heightIncrease = 4;
        //}
        //else
        //{
        //    heightIncrease = 1;
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMaskGround))
        {
            //set below to zero surface increase if it's a pickupable
            normalToSurfaceIncrease = hit.normal * heightIncrease;
            //bool isOverPickupable = false;
            
            //it does not actually go into these
            //if (hit.collider.gameObject.layer == 9)//pickupable
            //{
            //    //normalToSurfaceIncrease = Vector3.zero;//doesn't work
            //    //Debug.Log("on pickupable");
            //    //normalToSurfaceIncrease *= 0.1f;
            //    isOverPickupable = true;
            //}
            //if (hit.collider.gameObject.layer == 8)
            //{
            //    //normalToSurfaceIncrease = Vector3.zero;//doesn't work
            //    //Debug.Log("on ground");
            //    //normalToSurfaceIncrease *= heightIncrease;
            //}


            //if right click then keep hand at it's point on initiating right click and rotate camera around that point
            if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
            {
                positionOfLastRightCick = /*isOverPickupable ? hit.point :*/ hit.point + normalToSurfaceIncrease;
            }
            if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
            {
                Hand.transform.position = positionOfLastRightCick;
                //Cursor.lockState = CursorLockMode.Locked;//won't work, will prevent rotation input
            }
            else
            {
                //handInstance.transform.position = hit.point + zAdded;
                Hand.transform.position = Vector3.Lerp(Hand.transform.position, /*isOverPickupable? hit.point :*/ hit.point + normalToSurfaceIncrease, lerpSpeed);
                //Cursor.lockState = CursorLockMode.None;
            }
        }

    }
}
