using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHandMovement : MonoBehaviour
{
    /// How sensitive is holding left click to move the camera about?
    public float dragSensitivity = 25f;
    /// How sensitive is holding right click to rotate the camera?
    public float rotateSensitivity = 500f;
    /// How sensitive is using the scroll wheel?
    public float scrollSensitivity = 150f;
    /// How close does the camera need to get to the position of movement?
    ///  Needs to be larger the larger that drag sensitivity is
    public float acceptableError = 0.5f;

    public Camera mainCamera;

    private Vector3 _movePoint;
    private bool _move = false;
    private bool _rotate = false;

    void LateUpdate()
    {
        MoveCamera();
        RotateCamera();
    }


    private void MoveCamera()
    {
        // Scroll
        float scrollDistance = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity * Time.deltaTime;
        transform.Translate(transform.forward * scrollDistance);

        // Move based on drag
        if (Input.GetMouseButton(0))
        {
            Ray c = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(c, out hit))
            {
                if (!_move)
                {
                    // If this is the first frame, we need to set the move point
                    _movePoint = hit.point;
                    _move = true;
                }
                else
                {
                    // If we're already holding down (ie. _move is not set) then move camera
                    // based on distance mouse is from move point
                    Vector3 distance = hit.point - _movePoint;
                    Vector3 movement = distance.normalized;

                    if (distance.magnitude > acceptableError)
                        transform.Translate(-movement * Time.deltaTime * dragSensitivity, Space.World);
                }
            }
        }
        else
        {
            _move = false;
        }
    }

    public void RotateCamera()
    {
        if (Input.GetMouseButton(1))
        {
            if (!_rotate)
            {
                Ray c = mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(c, out hit))
                {
                    // If this is the first frame, we need to set the move point
                    _movePoint = hit.point;
                    _rotate = true;
                }
            }
            else
            {
                // Get the mouse change from last update and rotate around the movePoint
                float rotateUp = Input.GetAxis("Mouse Y");
                float rotateSide = Input.GetAxis("Mouse X");

                transform.RotateAround(_movePoint, transform.right, -rotateUp * rotateSensitivity * Time.deltaTime);
                transform.RotateAround(_movePoint, Vector3.up, rotateSide * rotateSensitivity * Time.deltaTime);
            }
        }
        else
        {
            _rotate = false;
        }
    }
}
