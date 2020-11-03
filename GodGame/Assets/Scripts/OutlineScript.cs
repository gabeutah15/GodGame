using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineScript : MonoBehaviour
{
    [SerializeField]
    private Material outlineMaterial;
    [SerializeField]
    private float outlineScaleFactor;
    [SerializeField]
    private Color outlineColor;
    //[SerializeField]
    public bool rendered { get; set; }
    private Renderer outlineRenderer;
    private Renderer thisRenderer;
    [SerializeField]
    GameObject throwableVersionOfObject;
    PickupManager pickupManager;


    //might be better to do this with shaders later? is that compatible with URP? should I use HDRP instead?

    // Start is called before the first frame update
    void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
        thisRenderer = this.GetComponent<Renderer>();
        pickupManager = FindObjectOfType<PickupManager>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (rendered)
        {
            //eventually will need to only render one peasant at a time, currently you could potentially 
            //be colliding with multiple peasants at a time
            //so world hand needs some kind of currentlyHighlightedPeasant gameobject
            //and if there is a new trigger enter then it is set to a new peasant
            //or could make it so that is not possible with colliders
            if (Input.GetMouseButtonDown(0))
            {
                rendered = false;
                //thisRenderer.enabled = false;
                //InstantiateThrowableAndGrab();

                if (this.GetComponentInParent<PickupableNavMeshAgent>())
                {
                    GameObject pickUpabale = Instantiate(throwableVersionOfObject, this.transform.position, this.transform.rotation);
                    pickupManager.PickupNewNavMeshObject(this.GetComponentInParent<PickupableNavMeshAgent>().gameObject, pickUpabale);
                }

                if (this.GetComponentInParent<PickupablePhysicsObject>())
                {
                    pickupManager.PickupPhysicsObject(this.GetComponentInParent<PickupablePhysicsObject>().gameObject);
                    //GameObject pickUpabale = Instantiate(throwableVersionOfObject, this.transform.position, this.transform.rotation);
                    //pickupManager.PickupNewObject(this.GetComponentInParent<PickupableNavMeshAgent>().gameObject, pickUpabale);
                }
            }
        }
        outlineRenderer.enabled = rendered;
    }

    //void InstantiateThrowableAndGrab()
    //{
    //    Instantiate(throwableVersionOfObject, this.transform.position, this.transform.rotation);
    //    throwableVersionOfObject.transform.SetParent(WorldHand.handInstance.transform);
    //    //below would have to be in update, not necessary for now, but would be nece for peasant to move quickly into hand instead of just appear there
    //    //throwableVersionOfObject.transform.position = Vector3.Lerp(throwableVersionOfObject.transform.position, WorldHand.handInstance.transform.position, 0.1f);
    //}

    Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        Renderer rend = outlineObject.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", color);
        rend.material.SetFloat("_Scale", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        outlineObject.GetComponent<OutlineScript>().enabled = false;//will have to disable any other scripts on here too?
        outlineObject.GetComponent<Collider>().enabled = false;

        rend.enabled = false;

        return rend;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("GodHand"))
    //    {
    //        rendered = true;
    //        //OutlineScript outline = transform.GetComponent<OutlineScript>();
    //        //if (outline)
    //        //{
    //        //    outline.rendered = true;
    //        //}
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("GodHand"))
    //    {
    //        rendered = true;
    //        //OutlineScript outline = transform.GetComponent<OutlineScript>();
    //        //if (outline)
    //        //{
    //        //    outline.rendered = false;
    //        //}
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GodHand"))
        {
            OutlineScript outline = transform.GetComponent<OutlineScript>();
            if (outline)
            {
                outline.rendered = true;
            }
        }
    }

    //for physics objects that need a rigidbody on the parent and so have a trigger to collider with outliner
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GodHand"))
        {
            OutlineScript outline = transform.GetComponent<OutlineScript>();
            if (outline)
            {
                outline.rendered = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("GodHand"))
        {
            transform.GetComponent<OutlineScript>().rendered = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GodHand"))
        {
            transform.GetComponent<OutlineScript>().rendered = false;
        }
    }
}
