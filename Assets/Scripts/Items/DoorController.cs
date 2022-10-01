using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    private GameObject sensor;
    public GameObject door;
    private Transform[] children;

    public delegate void Door(GameObject door);
    public Door doorChange;
    public float doorSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        children = gameObject.GetComponentsInChildren<Transform>();
        foreach(var i in children)
        {
            if(i.name=="sensor")
            {
                sensor = i.gameObject;
            }
            if(i.name== "LP_Bay_Door_snaps")
            {
                door = i.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void OpenDoor(GameObject door)
    {
        DoorController dc = door.GetComponentInParent<DoorController>();


        Tweener tweener = dc.door.transform.DOLocalMove(new Vector3(0, 5, 0), 3);
        tweener.SetEase(Ease.InCubic);
    }
    public static void CloseDoor(GameObject door)
    {
        DoorController dc = door.GetComponentInParent<DoorController>();
        Tweener tweener = dc.door.transform.DOLocalMove(new Vector3(0, 0, 0), 3);
        tweener.SetEase(Ease.InCubic);
    }

}
