using UnityEngine;
using UnityEngine.Extensions;
using System.Collections;
using UnityEngine.EventSystems;

public class StrateCam : MonoBehaviour
{
    // Public fields

    //added for cursor change when rotating

    public Texture2D CursorRotateLeft;
    public Texture2D CursorRotateRight;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Vector2 hotSpotRight = new Vector2(33, 0);
    [SerializeField]
    LayerMask layerMaskGround;
    [SerializeField]
    bool newRotation;


    //end added

    public int initialCameraRotationX = 35;
    public int initialCameraRotationY = -35;


    public Terrain terrain;

    public float panSpeed = 15.0f;
    public float zoomSpeed = 100.0f;
    public float rotationSpeed = 50.0f;

    public float mousePanMultiplier = 0.1f;
    public float mouseRotationMultiplier = 0.2f;
    public float mouseZoomMultiplier = 5.0f;

    public float minZoomDistance = 20.0f;
    private float normalMinZoom;
    private float doubledMinZoom;
    public float maxZoomDistance = 200.0f;
    public float smoothingFactor = 0.1f;
    public float goToSpeed = 0.1f;

    public bool useKeyboardInput = true;
    public bool useMouseInput = true;
    public bool adaptToTerrainHeight = true;
    public bool increaseSpeedWhenZoomedOut = true;
    public bool correctZoomingOutRatio = true;
    public bool smoothing = true;
    public bool allowDoubleClickMovement = false;

    public bool allowScreenEdgeMovement = true;
    public int screenEdgeSize = 10;
    public float screenEdgeSpeed = 1.0f;
    public float edgeMargin = 0f;

    public GameObject objectToFollow;
    public Vector3 cameraTarget;

    // private fields
    private float currentCameraDistance;
    private Vector3 lastMousePos;
    //private Vector3 handHitPoint;


    private Vector3 lastPanSpeed = Vector3.zero;
    private Vector3 goingToCameraTarget = Vector3.zero;
    private bool doingAutoMovement = false;
    private DoubleClickDetector doubleClickDetector;


    // Use this for initialization
    public void Start()
    {
        currentCameraDistance = minZoomDistance + ((maxZoomDistance - minZoomDistance) / 5.0f);
        lastMousePos = this.transform.position;// Vector3.zero;
        cameraTarget = this.transform.position;
        Cursor.visible = false;

        doubleClickDetector = GetComponent<DoubleClickDetector>();
        //Camera.main.transform.rotation = Quaternion.Euler(initialCameraRotationX, initialCameraRotationY, 1);
        Cursor.lockState = CursorLockMode.Confined;//i dunno if this should be in start or update but it has to be in a function
        normalMinZoom = minZoomDistance;
        doubledMinZoom = 2 * minZoomDistance;
    }

    // Update is called once per frame
    public void Update()
    {
        if (allowDoubleClickMovement)
        {
            UpdateDoubleClick();
        }
        UpdatePanning();
        UpdateRotation();
        UpdateZooming();
        UpdatePosition();
        UpdateAutoMovement();
        lastMousePos = Input.mousePosition;
    }

    public void GoTo(Vector3 position)
    {
        position.y = cameraTarget.y;
        doingAutoMovement = true;
        goingToCameraTarget = position;
        objectToFollow = null;
    }

    //public void Follow(GameObject gameObjectToFollow)
    //{
    //    objectToFollow = gameObjectToFollow;
    //}

    #region private functions
    private void UpdateDoubleClick()
    {
        if (doubleClickDetector.IsDoubleClickLeft() && terrain && terrain.GetComponent<Collider>())
        {
            var cameraTargetY = cameraTarget.y;

            var collider = terrain.GetComponent<Collider>();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            Vector3 pos;

            if (collider.Raycast(ray, out hit, Mathf.Infinity) && !EventSystem.current.IsPointerOverGameObject())
            {
                pos = hit.point;
                pos.y = cameraTargetY;
                GoTo(pos);
            }
        }
    }

    private void UpdatePanning()
    {

        Vector3 moveVector = new Vector3(0, 0, 0);

        //added
        float shiftMulti = 1.0f;

        float deltaAngleH = 0.0f;

        float deltaAngleV = 0.0f;



        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftMulti = 2.5f;
        }
        //added

        if (useKeyboardInput)
        {
            //! rewrite to adress xyz seperatly
            if (Input.GetKey(KeyCode.A))
            {
                moveVector.x -= 1 * shiftMulti;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveVector.z -= 1 * shiftMulti;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveVector.x += 1 * shiftMulti;
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveVector.z += 1 * shiftMulti;
            }
        }
        if (allowScreenEdgeMovement)
        {
            if (Input.mousePosition.x < screenEdgeSize && Input.mousePosition.y < .75 * (Screen.height))
            {
                moveVector.x -= screenEdgeSpeed * shiftMulti;
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
            }
            else if (Input.mousePosition.x > Screen.width - screenEdgeSize && Input.mousePosition.y < .75 * (Screen.height))
            {
                moveVector.x += screenEdgeSpeed * shiftMulti;
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
            }
            else if (Input.mousePosition.x < screenEdgeSize && Input.mousePosition.y >= .75 * (Screen.height))
            {
                deltaAngleH = -2.0f * shiftMulti * .7f;//THE shiftmulti is a bit fast for rotation so lowered it to 70% 
                Cursor.SetCursor(CursorRotateLeft, hotSpot, cursorMode);//changing the cursor
            }
            else if (Input.mousePosition.x > Screen.width - screenEdgeSize && Input.mousePosition.y >= .75 * (Screen.height))
            {
                deltaAngleH = 2.0f * shiftMulti * .7f;//and for rotating other way
                Cursor.SetCursor(CursorRotateRight, hotSpotRight, cursorMode);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
            }
            //FINISH rotating with mouse in corner section

            //added for deltaangle changed when moving mouse to corner
            transform.SetLocalEulerAngles(
                Mathf.Min(80.0f, Mathf.Max(5.0f, transform.localEulerAngles.x + deltaAngleV * Time.unscaledDeltaTime * rotationSpeed)),
                transform.localEulerAngles.y + deltaAngleH * Time.unscaledDeltaTime * rotationSpeed
                );

            //added

            if (Input.mousePosition.y < screenEdgeSize)
            {
                moveVector.z -= screenEdgeSpeed * shiftMulti;//more shiftmulti
            }
            else if (Input.mousePosition.y > Screen.height - screenEdgeSize)
            {
                moveVector.z += screenEdgeSpeed * shiftMulti;
            }
        }

        if (useMouseInput)
        {
            if (Input.GetMouseButton(2) /*&& Input.GetKey(KeyCode.LeftShift)*/)
            {
                Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);
                moveVector += new Vector3(-deltaMousePos.x, 0, -deltaMousePos.y) * mousePanMultiplier;
            }
        }

        if (moveVector != Vector3.zero)
        {
            objectToFollow = null;
            doingAutoMovement = false;
        }

        var effectivePanSpeed = moveVector;
        if (smoothing)
        {
            effectivePanSpeed = Vector3.Lerp(lastPanSpeed, moveVector, smoothingFactor);
            lastPanSpeed = effectivePanSpeed;
        }

        var oldXRotation = transform.localEulerAngles.x;

        // Set the local X rotation to 0;
        transform.SetLocalEulerAngles(0.0f);
        //transform.localEulerAngles = Vector3.zero;//added

        float panMultiplier = increaseSpeedWhenZoomedOut ? (Mathf.Sqrt(currentCameraDistance)) : 1.0f;
        cameraTarget = cameraTarget + transform.TransformDirection(effectivePanSpeed) * panSpeed * panMultiplier * Time.unscaledDeltaTime;

        // Set the old x rotation.
        transform.SetLocalEulerAngles(oldXRotation);
    }

    private void UpdateRotation()
    {

        float deltaAngleH = 0.0f;
        float deltaAngleV = 0.0f;

        float shiftMulti = 1.0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftMulti = 2.5f;
        }

        if (useKeyboardInput)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                deltaAngleH = -1.0f * shiftMulti;
            }
            if (Input.GetKey(KeyCode.E))
            {
                deltaAngleH = 1.0f * shiftMulti;
            }
        }

        if (useMouseInput)
        {
            if (Input.GetMouseButton(1))
            {

                Vector3 deltaMousePos = (Input.mousePosition - lastMousePos);

                deltaAngleH += deltaMousePos.x * mouseRotationMultiplier;
                deltaAngleV -= deltaMousePos.y * mouseRotationMultiplier;


            }

            if (Input.GetMouseButtonDown(1))
            {
                objectToFollow = WorldHand.handInstance;
                //set zoom to be distance from camera to worldhand so cannot use rotate to move?
                //float distanceToHand = Vector3.Distance(this.transform.position, WorldHand.handInstance.transform.position);
                //currentCameraDistance = Mathf.Max(minZoomDistance, Mathf.Min(maxZoomDistance, distanceToHand));
            }
            if (Input.GetMouseButtonUp(1))
            {
                objectToFollow = null;
            }
        }

        transform.SetLocalEulerAngles(
            Mathf.Min(80.0f, Mathf.Max(5.0f, transform.localEulerAngles.x + deltaAngleV * 0.02f * rotationSpeed)),
            transform.localEulerAngles.y + deltaAngleH * 0.02f * rotationSpeed
        );

    }

    private void UpdateZooming()
    {
        float deltaZoom = 0.0f;
        if (useKeyboardInput)
        {
            if (Input.GetKey(KeyCode.F))
            {
                deltaZoom = 1.0f;
            }
            if (Input.GetKey(KeyCode.R))
            {
                deltaZoom = -1.0f;
            }
        }
        if (useMouseInput)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            deltaZoom -= scroll * mouseZoomMultiplier;

            //ADDED FOR ORTHOGRAPHIC
            Camera.main.orthographicSize -= scroll;
        }
        var zoomedOutRatio = correctZoomingOutRatio ? (currentCameraDistance - minZoomDistance) / (maxZoomDistance - minZoomDistance) : 0.0f;
        currentCameraDistance = Mathf.Max(minZoomDistance/*objectToFollow? doubledMinZoom : normalMinZoom*/, Mathf.Min(maxZoomDistance, currentCameraDistance + deltaZoom * Time.unscaledDeltaTime * zoomSpeed * (zoomedOutRatio * 2.0f + 1.0f)));
    }

    private void UpdatePosition()
    {
        if (objectToFollow != null)
        {
            cameraTarget = Vector3.Lerp(cameraTarget, objectToFollow.transform.position, goToSpeed);
        }

        cameraTarget.x = Mathf.Clamp(cameraTarget.x, 0 - edgeMargin, terrain.terrainData.size.x + edgeMargin);
        cameraTarget.z = Mathf.Clamp(cameraTarget.z, 0 - edgeMargin, terrain.terrainData.size.z + edgeMargin);
        transform.position = cameraTarget;
        transform.Translate(Vector3.back * currentCameraDistance);

        if (adaptToTerrainHeight && terrain != null)
        {
            transform.SetPosition(
                null,
                Mathf.Max(terrain.SampleHeight(transform.position) + terrain.transform.position.y + 10.0f, transform.position.y)
            );
        }
    }

    private void UpdateAutoMovement()
    {
        if (doingAutoMovement)
        {
            cameraTarget = Vector3.Lerp(cameraTarget, goingToCameraTarget, goToSpeed);
            if (Vector3.Distance(goingToCameraTarget, cameraTarget) < 1.0f)
            {
                doingAutoMovement = false;
            }
        }
    }
    #endregion
}