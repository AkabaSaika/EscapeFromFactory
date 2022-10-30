using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorController : MonoBehaviour
{
    private GameObject sensor;
    public GameObject door;
    private Transform[] children;
    public float doorSpeed = 1.0f;
    public bool isOpen;
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

    public void OpenDoor()
    {
        isOpen=true;
        Tweener tweener = door.transform.DOLocalMove(new Vector3(0, 5, 0), 3);
        tweener.SetEase(Ease.InCubic);
    }

}
