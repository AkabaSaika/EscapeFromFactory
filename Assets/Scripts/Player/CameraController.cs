using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{   
    private float angleX;//マウスがX軸に沿って移動した角度  
    private float angleY;//マウスがY軸に沿って移動した角度
    private float offsetAngleX;
    private float offsetAngleY;
    private Vector3 offsetVector = new Vector3();//カメラの最終位置  
    private float radius = 10.0f;//カメラの移動半径
    private float mouseSensitivity = 0.5f;//マウス感度
    private Camera cam;
    [SerializeField]
    private Transform player;
    private Vector3 mouseEndPos;
    private Vector3 mouseStartPos;
    private float mouseScroll = 20.0f;
    [SerializeField]
    private Transform lookAtPoint;//マウスの注視位置

    
    private bool isLockOn;//カメラのロックオン状態
    [SerializeField]
    private float LockonTargetCameraDistance;
    [SerializeField]
    private float cameraHeightWhileLockon;

    private float boxCenter;
    private GameObject lockTarget;
    [SerializeField]
    private Image lockOnIcon;
    private PlayerController playerController;



    [SerializeField]
    private const float MAX_VERTICAL_ANGLE = 80.0f;
    [SerializeField]
    private const float MIN_VERTICAL_ANGLE = 1.0f;
    [SerializeField]
    private const float MAX_CAMERA_DISTANCE = 30.0f;
    [SerializeField]
    private const float MIN_CAMERA_DISTANCE = 1.0f;
    [SerializeField]
    private const float MAX_LOCKON_DINSTANCE = 10.0f;
    [SerializeField]
    private const float MAX_LOCKON_CAMERA_HEIGHT = 11.0f;
    [SerializeField]
    private const float MIN_LOCKON_CAMERA_HEIGHT = 1.0f;
    private const float ANGLE_CONVERTER = Mathf.PI / 180;

    public bool IsLockOn { get => isLockOn;}

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        mouseStartPos = Input.mousePosition;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //lookAtPoint = player;
        lookAtPoint = GameObject.Find("LookAtPoint").transform;
        boxCenter = 5.0f;
        
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
        
        if(IsLockOn)
        {
            lockOnIcon.rectTransform.position = cam.WorldToScreenPoint(lockTarget.transform.position+new Vector3(0,1.25f,0));
            Vector3 camPos = cam.transform.position;
            camPos.y = 0;
            LockonTargetCameraDistance = Vector3.Distance(lockTarget.transform.position, camPos);
            cameraHeightWhileLockon = MAX_LOCKON_CAMERA_HEIGHT * (1- (LockonTargetCameraDistance / MAX_LOCKON_DINSTANCE));
            cameraHeightWhileLockon = cameraHeightWhileLockon > MAX_LOCKON_CAMERA_HEIGHT ? MAX_LOCKON_CAMERA_HEIGHT : cameraHeightWhileLockon;
            cameraHeightWhileLockon = cameraHeightWhileLockon < MIN_LOCKON_CAMERA_HEIGHT ? MIN_LOCKON_CAMERA_HEIGHT : cameraHeightWhileLockon;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPos;
        Vector3 velocity =new Vector3(0,0,0);
        float soothtime = 0.1f;
        //if (lockTarget == null)
        //{
        //    RaycastHit hit;
        //    Ray ray = new Ray(lookAtPoint.position, lookAtPoint.position + offsetVector);
        //    if (Physics.Linecast(lookAtPoint.position, lookAtPoint.position + offsetVector, out hit))
        //    //if(Physics.SphereCast(ray,0.1f, out hit,10))
        //    {
        //        if (hit.collider.tag == "Environment")
        //        {
        //            cam.transform.position = hit.point;
        //            cam.transform.position += new Vector3(0, 0, 0.1f);
        //        }
        //        else cam.transform.position = lookAtPoint.position + offsetVector;
        //    }
        //    else cam.transform.position = lookAtPoint.position + offsetVector;

        //    cam.transform.LookAt(lookAtPoint);
        //}
        if (lockTarget == null)
        {
            RaycastHit hit;
            Ray ray = new Ray(lookAtPoint.position, lookAtPoint.position + offsetVector);
            if (Physics.Linecast(lookAtPoint.position, lookAtPoint.position + offsetVector, out hit))
            //if(Physics.SphereCast(ray,0.1f, out hit,10))
            {
                if (hit.collider.tag == "Environment")
                {
                    targetPos = hit.point;
                    targetPos += new Vector3(0, 0, 0.1f);
                }
                else targetPos = lookAtPoint.position + offsetVector;
            }
            else targetPos = lookAtPoint.position + offsetVector;
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPos, ref velocity, soothtime);
            cam.transform.LookAt(lookAtPoint);
        }
        else
        {
            Vector3 tmpFoward = Vector3.ProjectOnPlane(lockTarget.transform.position - player.transform.position,Vector3.up);
            tmpFoward.y = 0;
            player.transform.forward = tmpFoward;
            
            //cam.transform.position = player.transform.position + player.transform.forward * (-3) + player.transform.up*cameraHeightWhileLockon;
            targetPos= player.transform.position + player.transform.forward * (-3) + player.transform.up * cameraHeightWhileLockon;
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPos, ref velocity, soothtime);
            cam.transform.LookAt(lockTarget.transform.position);
        }

        
        
    }

    public void LockOn()
    {
        Vector3 tmpPlayerCenter = player.transform.position + Vector3.up;
        Vector3 tmpBoxCenter = tmpPlayerCenter + player.transform.forward * boxCenter;
        Collider[] colliders = Physics.OverlapBox(tmpBoxCenter, new Vector3(1.0f, 1.0f, 5.0f), player.transform.rotation,1<<12);
        if(colliders.Length!=0&&lockTarget==null)
        {
            lockTarget = colliders[0].gameObject;
            lockOnIcon.enabled = true;
            lockOnIcon.rectTransform.position = cam.WorldToScreenPoint(lockTarget.transform.position + Vector3.up);
            isLockOn = true;
        }
        else
        {
            lockTarget = null;
            lockOnIcon.enabled = false;
            isLockOn = false;
        }
    }
}
