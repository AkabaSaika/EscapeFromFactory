using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float angleX;
    private float angleY;
    private float offsetAngleX;
    private float offsetAngleY;
    private Vector3 offsetVector = new Vector3();
    private float radius = 10.0f;
    private float mouseSensitivity = 0.5f;
    private Camera cam;
    private Transform player;
    private Vector3 mouseEndPos;
    private Vector3 mouseStartPos;
    private float mouseScroll = 20.0f;
    [SerializeField]
    private Transform lookAtPoint;

    [SerializeField]
    private float MAX_VERTICAL_ANGLE = 80.0f;
    [SerializeField]
    private float MIN_VERTICAL_ANGLE = 1.0f;
    [SerializeField]
    private float MAX_CAMERA_DISTANCE = 30.0f;
    [SerializeField]
    private float MIN_CAMERA_DISTANCE = 1.0f;
    private float ANGLE_CONVERTER = Mathf.PI / 180;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        mouseStartPos = Input.mousePosition;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //lookAtPoint = player;
        lookAtPoint = GameObject.Find("LookAtPoint").transform;
    }

    // Update is called once per frame
    void Update()
    {
        mouseStartPos = mouseEndPos;
        mouseEndPos = Input.mousePosition;
        
        angleY = (mouseEndPos.y-mouseStartPos.y) * mouseSensitivity;
        angleX = (mouseEndPos.x-mouseStartPos.x) * mouseSensitivity;

        if(angleX!=0)
        {
            offsetAngleX += angleX;
        }
        if(angleY!=0)
        {
            offsetAngleY += angleY;
            offsetAngleY = offsetAngleY > MAX_VERTICAL_ANGLE ? MAX_VERTICAL_ANGLE : offsetAngleY;
            offsetAngleY = offsetAngleY < MIN_VERTICAL_ANGLE ? MIN_VERTICAL_ANGLE : offsetAngleY;
        }
        offsetVector.y = radius * Mathf.Sin(offsetAngleY * ANGLE_CONVERTER);
        float newRadius = radius * Mathf.Cos(offsetAngleY * ANGLE_CONVERTER);
        offsetVector.x = newRadius * Mathf.Sin(offsetAngleX * ANGLE_CONVERTER);
        offsetVector.z = -newRadius * Mathf.Cos(offsetAngleX * ANGLE_CONVERTER);

       

        cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * mouseScroll;
        cam.fieldOfView = cam.fieldOfView > MAX_CAMERA_DISTANCE ? MAX_CAMERA_DISTANCE : cam.fieldOfView;
        cam.fieldOfView = cam.fieldOfView < MIN_CAMERA_DISTANCE ? MIN_CAMERA_DISTANCE : cam.fieldOfView;
        
    }

    private void LateUpdate()
    {
        int layA = LayerMask.NameToLayer("Default");
        RaycastHit hit;
        if (Physics.Linecast(lookAtPoint.position, cam.transform.position, out hit))
        {
            if (hit.collider.tag == "Environment")
            {
                Debug.Log("hitwall");
                cam.transform.position = hit.point;
            }
            else cam.transform.position = lookAtPoint.position + offsetVector;
        }
        else cam.transform.position = lookAtPoint.position + offsetVector;
        cam.transform.LookAt(lookAtPoint);
    }


}
