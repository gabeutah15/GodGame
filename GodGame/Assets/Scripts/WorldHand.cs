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
    public static GameObject handInstance;
    Vector3 zAdded = new Vector3(0,0,0);
    Vector3 positionOfLastRightCick = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        handInstance = Instantiate(handPrefab);
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, layerMaskGround))
        {
            zAdded = hit.normal;
            //if right click then keep hand at it's point on initiating right click and rotate camera around that point
            if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
            {
                positionOfLastRightCick = hit.point + zAdded;
            }
            if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
            {
                handInstance.transform.position = positionOfLastRightCick;
            }
            else
            {
                handInstance.transform.position = hit.point + zAdded;
            }
        }

    }
}
